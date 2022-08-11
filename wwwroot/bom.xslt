<?xml version="1.0" encoding="UTF-8"?>
    <xsl:stylesheet 
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:xs="http://www.w3.org/2001/XMLSchema"
                xmlns:dc="https://jeremylong.github.io/DependencyCheck/dependency-check.2.5.xsd"
                xmlns:bom="http://cyclonedx.org/schema/bom/1.2"
                version="1.0"
                >
    <xsl:output version="1.0" encoding="UTF-8" method="xml" indent="yes" cdata-section-elements="bom:description"/>

        <xsl:template match="/">
        <bom xmlns="http://cyclonedx.org/schema/bom/1.2" serialNumber="urn:uuid:badcafe1-5bad-dddd-ea70-ca43be4cafe1" version="1">
            <metadata>
                <timestamp>
                    <xsl:value-of select="//dc:reportDate"/>
                </timestamp>
                <tools>
                    <tool>
                        <vendor>DepCheck2CycloneDX XSLT Converter</vendor>
                        <name>Dependency-Check to CycloneDX BOM XSLT converter</name>
                        <version>2.0.3</version>
                        <hashes>
                            <hash alg="MD5">8a41309418ecb39386d9b7df7beb69ae</hash>
                            <hash alg="SHA-1">3ff08273a4904414dd15ff441817bd926f72cda1</hash>
                            <hash alg="SHA-256">c9bf709fd770bb05bf811e86ab16d8e0ba132c5b0352d18eaa2d5d3800ab4afe</hash>
                        </hashes>
                    </tool>
                </tools>
            </metadata>
            <components>

            <!-- 
            Convert all dependencies identified in the dependency-check BOM 
            -->
            <xsl:for-each select="//dc:dependency">

            <!--
            But only work on dependencies that have a package identifier, as
            others cannot be processed by dependency-track and usually do not
            contribute to the vulnerability score anyway.
            -->
            <xsl:variable name="id" select="dc:identifiers/dc:package/dc:id"/>
            <xsl:if test="string-length($id) &gt; 0">
                <component bom-ref="{dc:identifiers/dc:package/dc:id}" type="library">
                        <!-- 
                        extract publisher, group, name and version from
                        package identifier 
                        e.g. pkg:maven/org.apache.tomcat/tomcat-jdbc@9.0.36
                        See "split" template below for logic to do that.
                        -->
                        <xsl:call-template name="split" >
                            <xsl:with-param name="value" select="$id"/>
                        </xsl:call-template>

                        <!-- 
                        copy over the hashes of the found artifacts 
                        -->
                        <hashes>
                            <hash alg="MD5"><xsl:value-of select="dc:md5"/></hash>
                            <hash alg="SHA-1"><xsl:value-of select="dc:sha1"/></hash>
                            <hash alg="SHA-256"><xsl:value-of select="dc:sha256"/></hash>
                        </hashes>

                        <!-- 
                        copy over license information
                        -->
                        <licenses>
                            <license>
                                <id><xsl:value-of select="dc:license"/></id>
                            </license>
                        </licenses>

                        <!--
                        Copy over description. Description needs to be in a
                        CDATA field, which is defined in the XSLT header above
                        <xsl:output ... cdata-section-elements="bom:description"/>
                        -->
                        <description><xsl:value-of select="dc:description"/></description>

                        <!--
                        copy over external references to vulnerability reports
                        -->
                        <externalReferences>
                        <xsl:for-each select="dc:vulnerabilities">
                        <xsl:for-each select="dc:vulnerability">
                        <xsl:for-each select="dc:references">
                        <xsl:for-each select="dc:reference">
                                    <reference type="advisories">
                                        <url><xsl:value-of select="dc:url"/></url>
                                    </reference>
                        </xsl:for-each>
                        </xsl:for-each>
                        </xsl:for-each>
                        </xsl:for-each>
                        </externalReferences>
                </component>
            </xsl:if>
            </xsl:for-each>
            </components>
        </bom>
    </xsl:template>


    <!-- 
    The "split" template extracts the package information based on the type of
    package found. It currently supports npm, javascript and maven
    dependencies.

    The xmlns tag is required because if it's not given, then the output has an
    empty xmlns appended for each element. This could then break BOM
    processing.
    -->
    <xsl:template name="split" xmlns="http://cyclonedx.org/schema/bom/1.2">
        <xsl:param name="value"/>
        <!-- 
        We have to differentiate between npm, javascript and maven
        dependencies. They all have different pkg formats in dependency-track
        -->
        <xsl:choose>
            <xsl:when test="contains($value, '%40')">
                <!-- npm -->
                <xsl:variable name="pub" select="substring-after(substring-before($value, '/'), ':')"/>
                <xsl:variable name="grp" select="substring-before(substring-after($value, '/%40'), '%2F')"/>
                <xsl:variable name="name" select="substring-before(substring-after($value, '%2F'), '@')"/>
                <xsl:variable name="ver" select="substring-after($value, '@')"/>
                <publisher><xsl:value-of select="$pub"/></publisher>
                <group><xsl:value-of select="$grp"/></group>
                <name><xsl:value-of select="$name"/></name>
                <version><xsl:value-of select="$ver"/></version>
                <purl>pkg:npm/@<xsl:value-of select="$grp"/>/<xsl:value-of select="$name"/>@<xsl:value-of select="$ver"/></purl>
            </xsl:when>
            <xsl:when test="contains($value, 'javascript')">
                <!-- javascript -->
                <xsl:variable name="pub" select="substring-after(substring-before($value, '/'), ':')"/>
                <xsl:variable name="name" select="substring-before(substring-after($value, '/'), '@')"/>
                <publisher><xsl:value-of select="$pub"/></publisher>
                <name><xsl:value-of select="$name"/></name>
                <version><xsl:value-of select="substring-after($value, '@')"/></version>
                <purl><xsl:value-of select="$value"/></purl>
            </xsl:when>
            <xsl:otherwise>
                <!-- maven -->
                <xsl:variable name="pub" select="substring-after(substring-before($value, '/'), ':')"/>
                <xsl:variable name="grp" select="substring-before(substring-after($value, '/'), '/')"/>
                <xsl:variable name="name" select="substring-before(substring-after(substring-after($value, '/'), '/'), '@')"/>
                <publisher><xsl:value-of select="$pub"/></publisher>
                <group><xsl:value-of select="$grp"/></group>
                <name><xsl:value-of select="$name"/></name>
                <version><xsl:value-of select="substring-after($value, '@')"/></version>
                <purl><xsl:value-of select="$value"/></purl>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>

</xsl:stylesheet>