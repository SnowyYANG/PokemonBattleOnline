<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="text" indent="yes"/>

    <xsl:template match="data">
        <xsl:variable name="dataname" select="@name"/>
        
        public void Save(Stream output)
        {
            BinaryWriter writer = new BinaryWriter(output);
            <xsl:for-each select="field[@convert]">
            writer.Write((<xsl:value-of select="@type"/>)<xsl:value-of select="@name"/>);
            </xsl:for-each>
            <xsl:for-each select="field[not (@convert)]">
            writer.Write(<xsl:value-of select="@name"/>);
            </xsl:for-each>
        }
        public static <xsl:value-of select ="$dataname"/> FromStream(Stream input)
        {
            <xsl:value-of select ="$dataname"/> data = new <xsl:value-of select ="$dataname"/>();
            BinaryReader reader = new BinaryReader(input);
            <xsl:for-each select="field[@convert]">
            data.<xsl:value-of select="@name"/> = (<xsl:value-of select="@convert"/>)reader.Read<xsl:value-of select="@type"/>();
            </xsl:for-each>
            <xsl:for-each select="field[not (@convert)]">
            data.<xsl:value-of select="@name"/> = reader.Read<xsl:value-of select="@type"/>();
            </xsl:for-each>
        
            return data;
        }
    </xsl:template>
</xsl:stylesheet>
