# DotLocal

![AppVeyor Build](https://ci.appveyor.com/api/projects/status/1hpq56xg4xovwu9d)

A simple local web server written for the .NET framework. It provides just enough functionality to be awesome.

## Options

```
-p, --port             The port number which should be used.

-r, --root             The root path of the web folder.

-d, --defaultFile      The default file that will be served to the user on
					 the root URL.

-l, --enableListing    Whether directory listing should be enabled or not.

-o, --enableLogging    Whether the logging of calls should be written to the
					 console.

-u, --username         The username that should be used for optional basic
					 authentication.

-w, --password         The password that should be used for optional basic
					 authentication.

--help                 Display this help screen.
```

## Example

`dotlocal -p 8080 -r "C:\WebRoot" -o true`