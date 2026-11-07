$stream = [System.IO.File]::OpenRead('Assembly-CSharp.dll')
$peReader = [System.Reflection.PortableExecutable.PEReader]::new($stream)
$meta = [System.Reflection.Metadata.PEReaderExtensions]::GetMetadataReader($peReader)
$types = foreach ($h in $meta.TypeDefinitions) {
    $t = $meta.GetTypeDefinition($h)
    $ns = $meta.GetString($t.Namespace)
    $n = $meta.GetString($t.Name)
    $fn = if ([string]::IsNullOrEmpty($ns)) { $n } else { "$ns.$n" }
    [PSCustomObject]@{
        FullName = $fn
        Name = $n
        Namespace = $ns
    }
}
$stream.Close()
$types | Export-Csv -Path 'all_types.csv' -NoTypeInformation
Write-Host "Total types found: $($types.Count)"
$keywords = @('Player', 'Club', 'Finance', 'Budget', 'Money', 'Cash', 'Manager', 'Match', 'Tactics', 'Save', 'Game', 'Scout', 'Transfer', 'Youth', 'Training', 'Stadium', 'Contract')
foreach ($kw in $keywords) {
    $matched = $types | Where-Object { $_.Name -like "*$kw*" }
    Write-Host "Keyword '$kw': $($matched.Count) types"
}

