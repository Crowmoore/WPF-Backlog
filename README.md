# WPF-Backlog

## What even is this so called Backlog?

This application is designed to help users keep track of their games. It aims to be simple and fast to use with an intuitive user interface. Users will be able to store their games into a MySQL database and get graphical presentation about their game completion rate. This application is written in C# using .NET framework and it is done as a part of Windows Programming course at JAMK.
(Made by a gamer, for gamers)

## Who would do such a thing?

I, Joel Kortelainen, am a 2nd year software engineering student at Jamk University of Applied Sciences. I am a human with two legs and two hands and some other stuff not worth mentioning.

## Installation

In order to get the Backlog up and running you'll need a Windows machine with .Net Framework 4.5.2 or newer.

If you have access to Labranet you can find the installation package from \\\ghost\TEMP\Installation\H3090.
Alternatively you can just grab the Release folder from this repository.

There's just one caveat; Since the MySQL database is running on the student server, in order to actually use the application you have to be connected with the Labranet, either directly or via VPN.

### Note

The config and settings files are not included in the source files since they contain some sensitive information about the database.

## Features

### Implemented

- Registering system to make you feel special and unique.
- MySQL database to store your data.
- Fully implemented CRUD system for data managing.
- Graphical presentation of your game completion rates. (It's really there just to make you feel bad)
- Color theme; you can select your favourite color (Hot Pink) from a huge variety of choices.
- Data filtering; you can search your games by title, genre or completion status.
- Status bar at the bottom to inform you if/when things go horribly wrong.
 
## Not implemented

- E-mail verification system when registering a new account.
- Showing the cover art for the games.

## What does it look like? Is it pretty?

### The Backlog

Beaty is in the eye of the beholder so I'll let you decide.

![alt tag](http://i.imgur.com/t0hkCzq.png)

![alt tag](http://i.imgur.com/JoEjCjp.png)

![alt tag](http://i.imgur.com/1YM6LGc.png)

### The database

The database that the Backlog uses is extremely simple. And simple equals beautifull. Right?

![alt tag](http://i.imgur.com/dRsYfE3.png)

## But how do I use it?

### First time launch

Using the application is as simple as riding a tricycle. Start the application and click the button labeled 'Register'. Still with me?
Good! Now comes the hard part; you need to come up with a clever username and also a password but that's not all. You also need
to type in your email address. If the username or email address is invalid, the text turns red, as seen below.

![alt tag](http://i.imgur.com/iF4fsu6.png)

If everything is a-okay, the text turns green. Easy!

![alt tag](http://i.imgur.com/LkWiSQx.png)

Once that has been taken care of you can log in using your newly created account and start adding
your games to the database. How great is that!?

### Adding, removing and updating

You can add a new game to the database by navigating to the 'Add game' tab and filling out the form presented before you.
To update the data of a single game you can write the changes directly to the datagrid and right click. This opens up a small
menu with two options Update and Delete. It looks like this:

![alt tag](http://i.imgur.com/0ey2dZY.png)

I suggest you choose Update at this point. And you guessed it, by selecting Delete the selected game will be permanently deleted from the database. But worry not, there is a confirmation box that prevents any nasty mishaps.

### Changing the color

This is the part you were waiting for. In order to change the main color of the Backlog just click the cog icon found in the upper right corner of the main view. This opens a list of colors for you to choose from.

![alt tag](http://i.imgur.com/H4Ig8qP.png)

## Time invested in the project

The total time it took to complete this project is around 35-40 hours. Not including the database creation.

## Future development

It's clear that there is a lot of room for improvement, for example more professional looking UI and better database management. But I would say that adding those features in this program would be a moot effort. I would prefec to start the project from scratch using the entity framework and looking up some similar software for UI ideas. As for now, I won't be developing the Backlog any further.

## Analysis

### What I learned during this project

I was somewhat familiar with C# before this project but I had no clue about the .NET framework. And during this project I learned a lot of those .NET shenanigans. Now that I'm older and wiser I would not go the route I took when creating an application like this.
There's a magical thing called the Entity Framework which would simplify the whole process of database interactions. But by doing things the way I did also taught me that said things can be done the easy way or the hard way. And isn't the hard way the best for learning purposes?

### Greatest challenge during the project

Interestingly enough, the greatest challenge I faced had very little to do with the actual program. Since I'm developing a web based version of the Backlog using PHP, I wanted that both services would work side by side. For example if a user registers to the Backlog using the PHP version, he/she would be able access his/hers games using the desktop version. Makes sense, right?
I decided to use MD5 hashing algorithm to hash the user's password but alas, to my dismay I noticed that the hashes between those two languages were slightly different. This caused a lot of head scrathing and cursing, but eventually I was able to defeat the beast that was language barrier.

## Review

I'm fairly pleased of how the program ended up. Everything works as it should and during the last tests I was unable to get the program to crash, so that's good. There's a lot of room for improvement on the UI side but since I'm not a graphical designer, meh.
Overall good stuff and I would rate the program 4/5 on an amateur scale.
