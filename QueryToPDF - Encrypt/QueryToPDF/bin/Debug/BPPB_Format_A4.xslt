<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
	<xsl:output method="html" omit-xml-declaration="yes" indent="yes"/>

	<xsl:template match="@* | node()">
		<html>
			<head>
				<style>
					body {
					padding: 0px;
					margin: 0px;
					border: 0px;
					}
					table {
					padding: 0px;
					margin: 0px;
					<!--border-bottom: 1px  solid black;-->
					width: 100%;
					font-size:20px;
					}
					th{
					border-bottom: 1px solid black;
					font-size:20px;
					}
					td{
					font-size:20px;
					}
					h6 .header{
					text-align:center;
					font-size:20px;
					}
					h6{
					font-size:20px;
					}
					h5 {
					margin-left: 0;
					font-size:20px;
					}
				</style>
			</head>
			<body>
				<!--<h6 class="header" style="text-align:center;">PT HM SAMPOERNA Tbk. Sales Office WE Sidoarjo</h6>
				<h6 class="header" style="text-align:center;">JL. BERBEK INDUSTRI VII 16-18, SIDOARJO</h6>
				<h6 class="header" style="text-align:center;">******* Bon Pengambilan dan Pengembalian Barang *******</h6>-->
				<h6 class="header" style="text-align:center;">
					<xsl:for-each select="*">
						<xsl:if test="position()=1">
							<xsl:for-each select="*">
								<xsl:if test="position()=2">
										<xsl:value-of select="."/>
								</xsl:if>
							</xsl:for-each>
						</xsl:if>
					</xsl:for-each>
					<br></br>
					<xsl:for-each select="*">
						<xsl:if test="position()=1">
							<xsl:for-each select="*">
								<xsl:if test="position()=3">
										<xsl:value-of select="."/>
								</xsl:if>
							</xsl:for-each>
						</xsl:if>
					</xsl:for-each>
					<br></br>
					<br></br>
					<br></br>
					******* Bon Pengambilan dan Pengembalian Barang *******
				</h6>
				<!--<h6 class="header" style="text-align:center;">******* Bon Pengambilan dan Pengembalian Barang *******</h6>-->
				<table style="margin-left:auto; margin-right:auto; width:100%">
					<tbody>
						<tr>
							<td colspan="2">
								Status Kunjungan
							</td>
							<td colspan="7">
								: Selesai
							</td>
						</tr>
						<tr>
							<td colspan="2">
								Nama Salesman
							</td>
							<td colspan="7">
								<xsl:for-each select="*">
									<xsl:if test="position()=1">
										<xsl:for-each select="*">
											<xsl:if test="position()=4">
												: <xsl:value-of select="."/>
											</xsl:if>
										</xsl:for-each>
									</xsl:if>
								</xsl:for-each>
							</td>
						</tr>
						<tr>
							<td colspan="2">
								ID SALESMAN
							</td>
							<td colspan="7">
								<xsl:for-each select="*">
									<xsl:if test="position()=1">
										<xsl:for-each select="*">
											<xsl:if test="position()=5">
												: <xsl:value-of select="."/>
											</xsl:if>
										</xsl:for-each>
									</xsl:if>
								</xsl:for-each>
							</td>
						</tr>
						<tr>
							<td colspan="2">
								Tanggal
							</td>
							<td colspan="7">
								<xsl:for-each select="*">
									<xsl:if test="position()=1">
										<xsl:for-each select="*">
											<xsl:if test="position()=6">
												: <xsl:value-of select="."/>
											</xsl:if>
										</xsl:for-each>
									</xsl:if>
								</xsl:for-each>
							</td>
						</tr>
						<tr>
							<td colspan ="9"></td>
						</tr>
						<tr>
							<td colspan ="9"></td>
						</tr>
						<tr>
							<td colspan ="9"></td>
						</tr>
						<tr align="left" style="width:100px" >
							<th style="text-align:center;">
								PRODUK
							</th>
							<th style="text-align:right;">
								STOK AWAL
							</th>
							<th style="text-align:right;">
								TAMBAH
							</th>
							<th style="text-align:right;">
								KURANG
							</th>
							<th style="text-align:right;">
								JUAL
							</th>
							<th style="text-align:right;">
								RETURN OK
							</th>
							<th style="text-align:right;">
								RETURN BS
							</th>
							<th style="text-align:right;">
								RWD
							</th>
							<th style="text-align:right;">
								STOCK AKHIR
							</th>
						</tr>
						<xsl:if test="position()">
							<xsl:for-each select="*">
								<xsl:if test="position()">
									<tr align="left" style="width:100px">
										<xsl:for-each select="*">
											<xsl:if test="position()>6 and 16>position()">
												<xsl:if test="position()=7">
													<td>
														<xsl:if test="number(.) = .">
															<xsl:value-of select="format-number(., '###,##0;(###,##0)')"/>
														</xsl:if>
														<xsl:if test="number(.) != .">
															<xsl:value-of select="."/>
														</xsl:if>
													</td>
												</xsl:if>
												<xsl:if test="position()!=7">
													<td style="text-align:right">
														<xsl:if test="number(.) = .">
															<xsl:value-of select="format-number(., '###,##0;(###,##0)')"/>
														</xsl:if>
														<xsl:if test="number(.) != .">
															<xsl:value-of select="."/>
														</xsl:if>
													</td>
												</xsl:if>
											</xsl:if>
										</xsl:for-each>
									</tr>
								</xsl:if>
							</xsl:for-each>
						</xsl:if>
					</tbody>
				</table>
				<pre>
					<H6>
Tanda Tangan







Gudang																											Salesman
					</H6>
				</pre>

				
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
