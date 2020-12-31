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
						width: 76.2mm;
					}
					page {
						width: 76.2mm;
					}
					table {
						padding: 0px;
						margin: 0px;
						<!--border-bottom: 1px  solid black;-->
						width: 100%;
						font-size:8px;
					}
					th{
						border-bottom: 1px solid black;
						font-size:8px;
					}
					td{
						font-size:8px;
					}
					h6 .header{
						text-align:center;
						font-size:8px;
					}
					h6{
						font-size:8px;
					}
					h5 {
						margin-left: 0;
						font-size:8px;
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
								<xsl:if test="position()=5">
										<xsl:value-of select="."/>
								</xsl:if>
							</xsl:for-each>
						</xsl:if>
					</xsl:for-each>
					<br></br>
					<xsl:for-each select="*">
						<xsl:if test="position()=1">
							<xsl:for-each select="*">
								<xsl:if test="position()=6">
										<xsl:value-of select="."/>
								</xsl:if>
							</xsl:for-each>
						</xsl:if>
					</xsl:for-each>
					<br></br>
					<br></br>
					<br></br>
					*** STOCK SUMMARY REPORT ***
				</h6>
				<table style="margin-left:auto; margin-right:auto; width:100%">
					<tbody>
						<tr>
							<td colspan="2">
								Salesman
							</td>
							<td colspan="5">
								<xsl:for-each select="*">
									<xsl:if test="position()=1">
										<xsl:for-each select="*">
											<xsl:if test="position()=8">
													: <xsl:value-of select="."/> /
											</xsl:if>
										</xsl:for-each>
									</xsl:if>
								</xsl:for-each>
								<xsl:for-each select="*">
									<xsl:if test="position()=1">
										<xsl:for-each select="*">
											<xsl:if test="position()=7">
													<xsl:value-of select="."/>
											</xsl:if>
										</xsl:for-each>
									</xsl:if>
								</xsl:for-each>
							</td>
						</tr>
						<tr>
							<td colspan="2">
								Kunjungan/ Tanggal
							</td>
							<td colspan="5">
								<xsl:for-each select="*">
									<xsl:if test="position()=1">
										<xsl:for-each select="*">
											<xsl:if test="position()=4">
													: STARTED / <xsl:value-of select="."/>
											</xsl:if>
										</xsl:for-each>
									</xsl:if>
								</xsl:for-each>
							</td>
						</tr>
						<tr>
							<td colspan ="7"></td>
						</tr>
						<tr align="left" style="width:100px" >
							<th style="text-align:center;">
								INITIAL STOCK
							</th>
							<th style="text-align:right;">
								LOAD UNLOAD
							</th>
							<th style="text-align:right;">
								SALES
							</th>
							<th style="text-align:right;">
								RETUR OK
							</th>
							<th style="text-align:right;">
								RETUR BS
							</th>
							<th style="text-align:right;">
								RWD
							</th>
							<th style="text-align:right;">
								STOCK END
							</th>
						</tr>
						
						<!--<xsl:if test="position()">
							<xsl:for-each select="*">
								<xsl:if test="position()">
									<tr align="left" style="width:100px">
										<xsl:for-each select="*">
											<xsl:if test="position()>6 and 14>position()">
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
						</xsl:if>-->
						<xsl:if test="position()">
							<xsl:for-each select="*">
								<xsl:if test="position()">
									<tr align="left" style="width:100px">
										<xsl:for-each select="*">
											<!--INITIAL STOCK-->
											<xsl:if test="position()=9">
												<td>
													<xsl:value-of select="."/>
												</td>
											</xsl:if>
											<!--LOAD UNLOAD-->
											<xsl:if test="position()=10">
												<td>
													<xsl:value-of select="."/>
												</td>
											</xsl:if>
											<!--SALES-->
											<xsl:if test="position()=13">
												<td>
													<xsl:value-of select="."/>
												</td>
											</xsl:if>
											<!--RETUR OK-->
											<xsl:if test="position()=14">
												<td>
													<xsl:value-of select="."/>
												</td>
											</xsl:if>
											<!--RETUR BS-->
											<xsl:if test="position()=15">
												<td>
													<xsl:value-of select="."/>
												</td>
											</xsl:if>
											<!--RWD-->
											<xsl:if test="position()=16">
												<td>
													<xsl:value-of select="."/>
												</td>
											</xsl:if>
											<!--END STOCK-->
											<xsl:if test="position()=17">
												<td>
													<xsl:value-of select="."/>
												</td>
											</xsl:if>
										</xsl:for-each>
									</tr>
								</xsl:if>
							</xsl:for-each>
						</xsl:if>
					</tbody>
				</table>


				<h6>Rincian Tambah/Kurang :</h6>
				<table style="margin-left:auto; margin-right:auto; width:100%">
					<tbody>
						<tr align="left" style="width:100px" >
							<th style="text-align:center;">
								PRODUK
							</th>
							<th style="text-align:center;">
								TAMBAH
							</th>
							<th style="text-align:center;">
								KURANG
							</th>
							<th style="text-align:center;">
								V2IN
							</th>
							<th style="text-align:center;">
								V2OUT
							</th>
						</tr>
						<xsl:if test="position()">
							<xsl:for-each select="*">
								<xsl:if test="position()">
									<tr align="left" style="width:100px">
										<xsl:for-each select="*">
											<!--PRODUK-->
											<xsl:if test="position()=9">
												<td>
													<xsl:value-of select="."/>
												</td>
											</xsl:if>
											<!--TAMBAH-->
											<xsl:if test="position()=11">
												<td>
													<xsl:value-of select="."/>
												</td>
											</xsl:if>
											<!--KURANG-->
											<xsl:if test="position()=12">
												<td>
													<xsl:value-of select="."/>
												</td>
											</xsl:if>
											<!--V2VIN-->
											<xsl:if test="position()=18">
												<td>
													<xsl:value-of select="."/>
												</td>
											</xsl:if>
											<!--V2VOUT-->
											<xsl:if test="position()=19">
												<td>
													<xsl:value-of select="."/>
												</td>
											</xsl:if>
										</xsl:for-each>
									</tr>
								</xsl:if>
							</xsl:for-each>
						</xsl:if>
						
						<!--<xsl:if test="position()">
							<xsl:for-each select="*">
								<xsl:if test="position()">
									<tr align="left" style="width:100px">
										<xsl:for-each select="*">
											<xsl:if test="position()>6 and 12>position()">
												<xsl:if test="position()=7">
													<td>
														<xsl:if test="number(.) = .">
															--><!-- myNode is a number --><!--
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
															--><!-- myNode is a number --><!--
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
						</xsl:if>-->
					</tbody>
				</table>
				<h6>SIGNATURE</h6>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
