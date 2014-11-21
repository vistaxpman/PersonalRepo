function FindProxyForURL(url, host) {
  if (shExpMatch(url, "*.google.*/*")) {
    return "PROXY HKPROXY01:3128";
  }

  if (shExpMatch(url, "*.deploy.akamaitechnologies.com/*")) {
    return "PROXY HKPROXY01:3128";
  }

  return "DIRECT; PROXY HKPROXY01:3128;";
}