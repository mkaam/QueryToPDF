<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" omit-xml-declaration="yes" indent="yes"/>
  
  <xsl:template match="@* | node()">
    <html>
      <body>
        <table>         
          <tbody>
            <tr height="30">
              <td colspan="4">
                Report Penjualan Salesman
               </td>
            </tr>
            <tr BGCOLOR="#2F75B5" align="center" height="15">
              <xsl:for-each select="/*/node()">
                <xsl:if test="position()=1">
                  <xsl:for-each select="*">
                    <td>
                      <b>
                        <font color="#ffffff" face="Segoe UI" size="1">
                          <xsl:value-of select="local-name()"/>
                        </font>
                      </b>
                    </td>
                  </xsl:for-each>
                </xsl:if>
              </xsl:for-each>
            </tr>
            <xsl:for-each select="*">

              <xsl:choose>
                <xsl:when test="Division = 'Note: DSS12 Includes DLE12'">
                  <tr BGCOLOR="#FFFFFF" align="left" height="15">
                    <xsl:for-each select="*">
                      <xsl:if test="position()=1">
                        <td colspan="7" align="left" style="width:50px">
                          <font face="Segoe UI" size="1.5">
                            <xsl:value-of select="."/>
                          </font>
                        </td>
                      </xsl:if>
                    </xsl:for-each>
                  </tr>
                </xsl:when>
                
                <xsl:when test="Brand = 'DLE12'">
                  <tr BGCOLOR="#DDEBF7" align="right" height="15">
                    <xsl:for-each select="*">
                      <xsl:if test="position()=1">
                        <td align="right" style="width:50px">
                          <font face="Segoe UI" size="1.5">
                            <xsl:if test="number(.) = .">
                              <!-- myNode is a number -->
                              <xsl:value-of select="format-number(., '###,##0.00000;(###,##0.00000)')"/>
                            </xsl:if>
                            <xsl:if test="number(.) != .">
                              <xsl:value-of select="."/>
                            </xsl:if>
                          </font>
                        </td>
                      </xsl:if>
                      <xsl:if test="position()>1">
                        <td align="right" style="width:50px">
                          <font face="Segoe UI" size="1.5">
                            <xsl:if test="number(.) = .">
                              <!-- myNode is a number -->
                              <xsl:value-of select="format-number(., '###,##0.00000;(###,##0.00000)')"/>
                            </xsl:if>
                            <xsl:if test="number(.) != .">
                              <xsl:value-of select="."/>
                            </xsl:if>
                          </font>
                        </td>
                      </xsl:if>
                    </xsl:for-each>
                  </tr>
                </xsl:when>
                <xsl:when test="Brand = 'TOTAL'">
                  <tr BGCOLOR="#2F75B5" align="center" height="15">
                    <xsl:for-each select="*">
                      <xsl:if test="position()=1">
                        <td align="left" style="width:50px">
                          <font color="#ffffff" face="Segoe UI" size="1.5">
                            <xsl:if test="number(.) = .">
                              <!-- myNode is a number -->
                              <xsl:value-of select="format-number(., '###,##0.00000;(###,##0.00000)')"/>
                            </xsl:if>
                            <xsl:if test="number(.) != .">
                              <xsl:value-of select="."/>
                            </xsl:if>
                          </font>
                        </td>
                      </xsl:if>
                      <xsl:if test="position()>1">
                        <td align="right" style="width:50px">
                          <font color="#ffffff" face="Segoe UI" size="1.5">
                            <xsl:if test="number(.) = .">
                              <!-- myNode is a number -->
                              <xsl:value-of select="format-number(., '###,##0.00000;(###,##0.00000)')"/>
                            </xsl:if>
                            <xsl:if test="number(.) != .">
                              <xsl:value-of select="."/>
                            </xsl:if>
                          </font>
                        </td>
                      </xsl:if>
                    </xsl:for-each>
                  </tr>
                </xsl:when>
                
                <xsl:when test="Brand = 'Total'">
                  <tr BGCOLOR="#2F75B5" align="center" height="15">
                    <xsl:for-each select="*">
                      <xsl:if test="position()=1">
                        <td align="left" style="width:50px">
                          <font color="#ffffff" face="Segoe UI" size="1.5">
                            <xsl:if test="number(.) = .">
                              <!-- myNode is a number -->
                              <xsl:value-of select="format-number(., '###,##0.00000;(###,##0.00000)')"/>
                            </xsl:if>
                            <xsl:if test="number(.) != .">
                              <xsl:value-of select="."/>
                            </xsl:if>
                          </font>
                        </td>
                      </xsl:if>
                      <xsl:if test="position()>1">
                        <td align="right" style="width:50px">
                          <font color="#ffffff" face="Segoe UI" size="1.5">
                            <xsl:if test="number(.) = .">
                              <!-- myNode is a number -->
                              <xsl:value-of select="format-number(., '###,##0.00000;(###,##0.00000)')"/>
                            </xsl:if>
                            <xsl:if test="number(.) != .">
                              <xsl:value-of select="."/>
                            </xsl:if>
                          </font>
                        </td>
                      </xsl:if>
                    </xsl:for-each>
                  </tr>
                </xsl:when>
                <xsl:when test="Brand = ''">
                  <tr BGCOLOR="#FFFFFF" align="center" height="7">
                    <xsl:for-each select="*">
                      <xsl:if test="position()=1">
                        <td align="left" style="width:50px">
                          <font face="Segoe UI" size="1.5">
                            <xsl:value-of select="."/>
                          </font>
                        </td>
                      </xsl:if>
                      <xsl:if test="position()>1">
                        <td align="right" style="width:50px">
                          <font face="Segoe UI" size="1.5">
                            <xsl:value-of select="."/>
                          </font>
                        </td>
                      </xsl:if>
                    </xsl:for-each>
                  </tr>
                </xsl:when>
                <xsl:otherwise>
                  <tr height="15">
                    <xsl:for-each select="*">
                      <xsl:if test="position()=1">
                        <td bgcolor="#DDEBF7" align="left" style="width:50px">
                          
                          <font face="Segoe UI" size="1.5">
                            <xsl:if test="number(.) = .">
                              <!-- myNode is a number -->
                              <xsl:value-of select="format-number(., '###,##0.00000;(###,##0.00000)')"/>
                            </xsl:if>
                            <xsl:if test="number(.) != .">
                              <xsl:value-of select="."/>
                            </xsl:if>

                          </font>
                        </td>
                      </xsl:if>
                      <xsl:if test="position()>1">
                        <td bgcolor="#DDEBF7" align="right" style="width:50px">
                  
                            <xsl:choose>
                              <xsl:when test="number(.) &lt; 0">
                                <font color="#FF0000" face="Segoe UI" size="1.5">
                                  <xsl:if test="number(.) = .">
                                    <!-- myNode is a number -->

                                    <xsl:value-of select="format-number(., '###,##0.00000;(###,##0.00000)')"/>
                                  </xsl:if>
                                  <xsl:if test="number(.) != .">
                                    <xsl:value-of select="."/>
                                  </xsl:if>
                                </font>
                              </xsl:when>
                              <xsl:otherwise>
                                <font face="Segoe UI" size="1.5">
                                  <xsl:if test="number(.) = .">
                                    <!-- myNode is a number -->

                                    <xsl:value-of select="format-number(., '###,##0.00000;(###,##0.00000)')"/>
                                  </xsl:if>
                                  <xsl:if test="number(.) != .">
                                    <xsl:value-of select="."/>
                                  </xsl:if>
                                </font>
                              </xsl:otherwise>
                            </xsl:choose>

                        </td>
                      </xsl:if>
                    </xsl:for-each>
                  </tr>
                </xsl:otherwise>
              </xsl:choose>
              
            </xsl:for-each>
          </tbody>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
