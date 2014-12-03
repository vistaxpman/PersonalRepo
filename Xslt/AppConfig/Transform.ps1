$xslt =  New-Object System.Xml.Xsl.XslCompiledTransform;
$xslt.Load('AppConfig.xslt');
$xslt.Transform('App.config', 'App.config.new');