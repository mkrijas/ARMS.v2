param(
    [string[]]$Files
)

foreach ($file in $Files) {
    if (-not (Test-Path $file)) {
        Write-Host "File not found: $file"
        continue
    }

    [xml]$rdl = Get-Content $file
    $ns = New-Object System.Xml.XmlNamespaceManager($rdl.NameTable)
    $ns.AddNamespace("r", "http://schemas.microsoft.com/sqlserver/reporting/2016/01/reportdefinition")

    $modified = $false

    $tb119 = $rdl.SelectSingleNode("//r:Textbox[@Name='Textbox119']", $ns)
    if ($tb119) {
        $tb119.Top = "0.65in"
        $tb119.Left = "0.14534in"
        $tb119.Width = "6.5in"
        $tb119Align = $tb119.SelectSingleNode(".//r:Style/r:TextAlign", $ns)
        if ($tb119Align) { $tb119Align.InnerText = "Left" }
        $modified = $true
    }

    $tb120 = $rdl.SelectSingleNode("//r:Textbox[@Name='Textbox120']", $ns)
    if ($tb120) {
        $tb120.Top = "0.85in"
        $tb120.Left = "0.14534in"
        $tb120.Width = "6.5in"
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
        $modified = $true
    }

    if ($modified) {
        $settings = New-Object System.Xml.XmlWriterSettings
        $settings.Indent = $true
        $settings.Encoding = New-Object System.Text.UTF8Encoding($false)
        $writer = [System.Xml.XmlWriter]::Create((Get-Item $file).FullName, $settings)
        $rdl.Save($writer)
        $writer.Close()
        Write-Host "Updated RDL: $file"
    } else {
        Write-Host "No Textbox119/120 found in: $file"
    }
}
