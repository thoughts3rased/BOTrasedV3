
# Welcome to BOTrased's Repository!

Whether you're here to take a look, contribute or ended up here by accident, welcome!

# What is BOTrased?
BOTrased is a basic bot that aims to host a range of administrative tools, helpful utilities and a collection of text-based games. BOTrased is open-source, so you can browse, copy, modify and view the code however you like.

# Thinking of contributing?
Some guidelines for contributing:
- Pull requests are to be targeted to the development branch, not master
- If you're creating a command, make sure it fits with the "spirit" of the bot
- If you're fixing a bug, make sure to raise an issue for it first

# Running BOTrased
BOTrased is cross platform as it is .NET 8, but prefers to run in Docker.

You'll need an `appsettings.json` file - you'll have to create this yourself. Check the `Models/Configuration.cs` file to see what properties you'll need.

At a bare minimum, you'll need to provide a bot token, test guild ID and a connection string for your database.

Unless you're running on a production bot, ensure that you build BOTrased in debug mode.

Your database user that you use BOTrased with will need read/write permissions for all the tables in the schema.

You'll need a SQL Server instance in order to run BOTrased. Open the `BOTrased.Database` project in Visual Studio, and publish the database structure using SSDT. Other methods are NOT supported.
