Param(
    [Parameter(Position = 0,
    Mandatory = $true,
    ValueFromPipeline = $false)]
    [String]
    $PS_MODULE_NAME,
    [Parameter(Position = 1,
    Mandatory = $true,
    ValueFromPipeline = $false)]
    [String]
    $DOCKER_USERNAME,
    [Parameter(Position = 2,
    Mandatory = $true,
    ValueFromPipeline = $false)]
    [String]
    $DOCKER_IMAGE_NAME,
    [Parameter(Position = 3,
    Mandatory = $true,
    ValueFromPipeline = $false)]
    [Security.SecureString]
    $DOCKER_PASSWORD
)
$publishedImageVersions = (Invoke-RestMethod https://registry.hub.docker.com/v2/repositories/$DOCKER_USERNAME/$DOCKER_IMAGE_NAME/tags).results | % {
    $_.name
}
Find-Module $PS_MODULE_NAME -AllVersions -AllowPrerelease | % {
    $moduleVersion = $_.Version;
    if ( !( $publishedImageVersions -contains $moduleVersion ) ) {
        $moduleVersion
    }
}