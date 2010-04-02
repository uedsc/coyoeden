Developing mojoPortal with MonoDevelop
======================================

Opening mojoPortal in MonoDevelop
---------------------------------
To open the mojoPortal solution and all it's projects, run MonoDevelop and
open mojoportal.mds.

Building mojoPortal
------------------- 

To build mojoPortal, select the mojoportal solution, and press F7.  You can
expect to get many warnings but hopefully no errors.

One-time setup to allow creation of a test deployment
-----------------------------------------------------

To create the deployment zip files and do a test deployment, first do the
following one-time setup:

1. Ensure that xsp is installed and postgresql is installed and running.

2. As the postgres user, create a postgresql user for yourself:
		createuser --createdb <your_username>

3. As the postgres user, create a postgresql user named mojo with password mojo: 
		createuser --pwprompt --no-adduser --no-createdb mojo

4. As the postgres user, ensure that mojo can connect through tcpip:

	4a. Uncomment the tcp_ip line in /var/lib/pgsql/data/postgresql.conf:

		tcpip_socket = true

	4b. Add this line to the bottom of /var/lib/pgsql/data/pg_hba.conf to
		create the least priveleged settings for mojo to connect from the local
		machine. (You can specify any other single IP or the range and subnet
		of your local network if you are going to connect from elsewhere.):

		host mojotest mojo 127.0.0.1 255.255.255.255 password

5. Make sure that your firewall is open to tcp traffic on 5432.

6. As root, restart postgresql: /etc/init.d/postgresql restart

7. As yourself, copy Web/Web.config to Deploy/Web.config, change the 
PostgreSQLConnectionString to refer to database mojotest and make any other
edits that are appropriate for your local environment.

8. As yourself, open mojoportal.mds in MonoDevelop and make mojoPortal.Deploy
   the startup project.  In MonoDevelop 0.7, you will need to do this 
   everytime you open mojoportal.mds.  In later versions, MonoDevelop 
   correctly remembers your startup project.  To set the startup project:

	8a. Right click on the mojoportal solution and select Options.

	8b. Under Startup Properties, select mojoPortal.Deploy as the Single 
		Startup project.  

Performing a test deployment
----------------------------

Now to actually create the deployment zip files and do a test deployment,
simply press F5.  The script which is executed will create 
Deploy/mojoportal-PostgreSQLBinaries.zip, unzip it to Deploy/test-inst, 
recreate the mojotest database, and restart xsp. When it finishes running, you
can point your browser at http://localhost:8080/ to see the test deployment.
In addition, the script will create Deploy/mojoportal-DataLayerForMySQL.zip
which MySQL users can manually deploy.

If you want to be able to access the test installation 
via http://mojoportal.localdomain/, do the following as root:

1. Add "mojoportal.localdomain" to the end of the 127.0.0.1 line
in /etc/hosts

2. Create /etc/httpd/conf.d/mod_proxy.conf
with the following contents (edited if necessary):

# Use name-based virtual hosting.
#
NameVirtualHost *:80
<VirtualHost *:80>
</VirtualHost>

<VirtualHost *:80>
    ServerName mojoportal.localdomain
    ProxyPreserveHost On
    ProxyRequests Off

    <Proxy *>
        Order deny,allow
        Deny from all
        Allow from 127.0.0.1
# Allow from other IPs or networks using lines like this:
#        Allow from 192.168.1
    </Proxy>

    ProxyPass / http://localhost:8080/
    ProxyPassReverse / http://localhost:8080/
</VirtualHost>


3. Restart apache: /etc/init.d/httpd restart
