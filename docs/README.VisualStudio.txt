General Notes for developers working with the Visual Studio Solutions

You can work with the mojoPortal Source Code using Visual Studio 2008 SP1 or the free Visual Web Developer Express 2008 SP1

For using Silverlight or MVC you may need to install additional items like the Silverlight Toolkit or MVC.

Note that we are targeting the .NET 3.5 runtime. If you need a 2.0 build, it may be possible (for a while) for you to change the build target an leave out files that won't compile.

mojoportal.sln has everything except experimental protype work with silverlight, this is the .sln most people should use
mojoportal_mssql_only.sln is the same as mojoportal.sln but removes the projects for databases other than MS SQL. If you only plan to use MS SQL then this solution will load faster and build faster without the additional data projects.
mojoportal-plus-silverlight.sln is same as mojoportal.sln but includes some silverlight projects that are in the protype/experimental stage, it requires VS 2008 SP1 with Silverlight Tools add on
mojoportal-core.sln is just the core web content management system with minumal features
mojoportal-silverlight-blend.sln is for working with the Silverlight projects in Expression Blend. It leaves out most other projects.
mojoportal-mvc-experimental.sln is just something I plan to use for exploratory work with MVC

More documentation is available here:
http://www.mojoportal.com/documentation.aspx
http://www.mojoportal.com/developerdocs.aspx
http://www.mojoportal.com/hello-world-developer-quick-start.aspx
http://www.mojoportal.com/developinginvisualstudio.aspx

Steps for working in VS Web Server
1 Setup your db according the the instruction for your db on mojoportal.com
2 Put your connection string in the web.config for your chosen db, optionally create a user.config file and put your connection string there instead of editing the one in web.config. This is a good idea if you are working from svn because you can update from svn without losing your connection string.
3 Check the references under mojoPortal.Business and see if it is referencing the data layer for the db you want to use, MS SQL, MySQL, PostgreSQL, or SQLite. If it is not referencing the one you want delete the incorrect reference, right click References and Add a Reference, click the Projects tab and set a Project Reference to the data layer for your db. Do the same for other "Business" projects like mojoPortal.Features.Business, WebStore.Business.
4 Check references also for mojoPortal.Features.Business, and WebStore.Business to use your preferred db.
5 Build the entire Solution 
6 Launch the VS Web server and visit /Setup/Default.aspx to complete setup
7 Enjoy :)
8 Spread the word


Steps for IIS setup
1 Setup your db according the the instruction for your db on mojoportal.com
2 Put your connection string in the web.config for your chosen db
3 Setup a Virtual Directory in IIS like http://localhost/mojoportal and point it to the Web folder
4 Right Click the Web\Data folder and on the Security tab and grant write permission to the ASPNET user or on Win 2003 the IIS_WPG user
5 Check the references under mojoPortal.Business and see if it is referencing the data layer for the db you want to use, MS SQL, MySQL, PostgreSQL, or SQLite. If it is not referencing the one you want delete the incorrect reference, right click References and Add a Reference, click the Projects tab and set a Project Reference to the data layer for your db.
6 Right click the Web project in VS and choose properties, then click the Web tab and select "Use IIS Web Server" and enter the url for the project so that it matches what you configured in IIS (http://localhost/mojoportal)
7 Build the entire Solution 
8 Visit http://localhost/mojoportal/Setup/Default.aspx to complete setup
9 Enjoy :)
10 Spread the word


Additional info:

You may notice when debugging that multiple web servers are spawned. This is because there are multiple web applications in the solution as features are split into separate projects. All the files for features get copied up to the main mojoPortal.Web project ie Web folder by post build events, so the feature projects are not meant to be run directly, the mojoPortal.Web project  must always be the startup project, though you can set breakpoints and debug any of the code in any of the projects, but the main web project always has to be the startup project. You can disable those extra web apps from launching a web server as described here http://stackoverflow.com/questions/16363/how-do-you-configure-vs2008-to-only-open-one-webserver-in-a-solution-with-multi#16390

Just click the project node in Solution Explorer and then click the Properties tab on the right and you will see where to disable it. I have disabled them on my copy but it does not seem to persist those settings in the solution, they are apparently user specific.
