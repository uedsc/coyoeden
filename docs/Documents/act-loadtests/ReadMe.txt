The ACT- Load tests all use the url http://loadtest/mojo as the starting point. This is to allow testing locally or on a different machine using these tests.
To use locally, add an entry in your hosts file (Windows\System32\drivers\etc\hosts
127.0.0.1	loadtest

or if using a remote web server, alias the ip of that server to loadtest like this:
172.16.0.4	loadtest

This test makes requests that correspond to the default site data created from a clean install of mojoportal

