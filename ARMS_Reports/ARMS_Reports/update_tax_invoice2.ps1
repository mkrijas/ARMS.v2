param([string]$FilePath)

[xml]$rdl = Get-Content $FilePath
$ns = New-Object System.Xml.XmlNamespaceManager($rdl.NameTable)
$ns.AddNamespace("r", "http://schemas.microsoft.com/sqlserver/reporting/2016/01/reportdefinition")

$tb119 = $rdl.SelectSingleNode("//r:Textbox[@Name='Textbox119']", $ns)
if ($tb119) {
    $tb119.Top = "0.65in"
    $tb119.Left = "0.14534in"
    $tb119.Width = "7.0in"
    $tb119Align = $tb119.SelectSingleNode(".//r:Style/r:TextAlign", $ns)
    if ($tb119Align) { $tb119Align.InnerText = "Left" }
}

$tb120 = $rdl.SelectSingleNode("//r:Textbox[@Name='Textbox120']", $ns)
if ($tb120) {
    $tb120.Top = "0.85in"
    $tb120.Left = "0.14534in"
    $tb120.Width = "7.0in"
    $tb120Aligns = $tb120.SelectNodes(".//r:Style/r:TextAlign", $ns)
    foreach ($align in $tb120Aligns) {
        if ($align.InnerText -eq 'Center') {
            $align.InnerText = "Left"
        }
    }
    $border = $tb120.SelectSingleNode(".//r:Style/r:Border/r:Style", $ns)
    if ($border -and $border.InnerText -eq "Solid") {
        $border.InnerText = "None"
    }
}

$r1 = $rdl.SelectSingleNode("//r:Rectangle[@Name='Rectangle1']", $ns)
if ($r1) {
    $r1.Height = "2.0in"
    $r1.Top = "0in"
}

$t27 = $rdl.SelectSingleNode("//r:Textbox[@Name='Textbox27']", $ns)
if ($t27) {
    $t27.Top = "2.0in"
}

$r2 = $rdl.SelectSingleNode("//r:Rectangle[@Name='Rectangle2']", $ns)
if ($r2) {
    $r2.Top = "2.25in"
}

$r9 = $rdl.SelectSingleNode("//r:Rectangle[@Name='Rectangle9']", $ns)
if ($r9) {
    $r9.Top = "3.5in"
}

$r8 = $rdl.SelectSingleNode("//r:Rectangle[@Name='Rectangle8']", $ns)
if ($r8) {
    $r8.Height = "11.23433in"
}

$body = $rdl.SelectSingleNode("//r:Body", $ns)
if ($body) {
    $body.Height = "28.57048cm"
}

$settings = New-Object System.Xml.XmlWriterSettings
$settings.Indent = $true
$settings.Encoding = New-Object System.Text.UTF8Encoding($false)
$writer = [System.Xml.XmlWriter]::Create($FilePath, $settings)
$rdl.Save($writer)
$writer.Close()

Write-Host "Updated RDL successfully."
