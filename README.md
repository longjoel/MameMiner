# Mame Miner

A tool for finding particular games burried in a massive archive of mame roms.

## Screenshot

![](http://i.imgur.com/Vfcu3Y5.png?raw=true)

## Download

* [Download](https://github.com/longjoel/MameMiner/releases/download/1.0/MameMiner.zip)
* [Documentation](https://github.com/longjoel/MameMiner/raw/master/Documentation/MameMinerDoc.pdf)
* These links will be revised as new versions are made available.

## Recent Changes

* 11/4/2015 - Updated documentation.
* 10/24/2015 - Added option to filter by good, working, and non-working games.
* 10/20/2015 - Added advanced search options.

## Mame Miner Use Cases

* You have an absolutely massive ammount of games, but only really want to find a hand full.
* You have absolutely no interest in filtering, filing, sorting, grouping, indexing, or cleaning. 
* You don't care about merged, unmerged, split, unsplit, parent roms, child roms.
* You just want games that will run.

## How Mame Miner Works

Mame Miner works by querying mame itself instead of querying the xml file mame can generate. That XML file can be over 100 MB
large and take a long time to parse. Mame's `-listfull` and `-listroms` command line options are very efficent. While the XML 
file generated by mame's `-listxml` contains significantly more data than the other commands, it contains data we are not 
interested in.

We are mostly concerned with the output of the `-listfull` and `-listroms` commands.

```
c:\Mame>mame64 -listfull pacman
Name:             Description:
pacman            "Pac-Man (Midway)"
```

```
c:\Mame>mame64 -listroms pacman
ROMs required for driver "pacman".
Name                    Size Checksum
pacman.6e               4096 CRC(c1e6ab10) SHA1(e87e059c5be45753f7e9f33dff851f16d6751181)
pacman.6f               4096 CRC(1a6fb2d4) SHA1(674d3a7f00d8be5e38b1fdc208ebef5a92d38329)
pacman.6h               4096 CRC(bcdd1beb) SHA1(8e47e8c2c4d6117d174cdac150392042d3e0a881)
pacman.6j               4096 CRC(817d94e3) SHA1(d4a70d56bb01d27d094d73db8667ffb00ca69cb9)
pacman.5e               4096 CRC(0c944964) SHA1(06ef227747a440831c9a3a613b76693d52a2f0a9)
pacman.5f               4096 CRC(958fedf9) SHA1(4a937ac02216ea8c96477d4a15522070507fb599)
82s123.7f                 32 CRC(2fc650bd) SHA1(8d0268dee78e47c712202b0ec4f1f51109b1f2a5)
82s126.4a                256 CRC(3eb3a8e4) SHA1(19097b5f60d1030f8b82d9f1d3a241f93e5c75d6)
82s126.1m                256 CRC(a9cc86bf) SHA1(bbcec0570aeceb582ff8238a4bc8546a23430081)
82s126.3m                256 CRC(77245b66) SHA1(0c4d0bee858b97632411c440bea6948a74759746)
```

This gives us everything we need in order to build games that work well with mame.

The other part of the equation is to create a database of scanned roms using a sqlite database. This database contains one 
table with the following columns.

`Archive Path char(255) | Rom file name char(80) | Size long | CRC 32 long | SHA 1 char(40)`

While the process to scan your rom collection can take a significant ammount of time, Mame Miner does so in a way
that will not impede the rest of your system. Other rom organizing tools are written in such a way that all the processing 
happens in the UI thread, causing Windows to believe the program is in a non-responsive state.

When this table is built, making queries against it is very fast. This in a nut-shell is how the entire process works.

* 1 - Mame generates a list of games and game descriptions at start up.
* 2 - User searches for a specific game name based on the game descriptions generated in step 1.
* 3 - Mame Miner queries mame for list of roms based on the game selected in step 2.
* 4 - Mame Miner then queries the database for the roms stored in the zip files based on rom name, file size, crc32 and sha1
* 5 - Mame Miner attempts to generate a new zip file based on the results in step 4.
* 6 - If step 5 is successfull, place generated zip file in the mame rom directory.

## How to code for Mame Miner

I really don't expect anybody to contribute to this, but if you have an interest in this project, send me a message first. 

## License

Copyright (c) 2015 Joel Longanecker

This software is provided 'as-is', without any express or implied warranty. In no event will the authors be held liable for 
any damages arising from the use of this software.

Permission is granted to anyone to use this software for any purpose, including commercial applications, 
and to alter it and redistribute it freely, subject to the following restrictions:

1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software. 
If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.

2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.

3. This notice may not be removed or altered from any source distribution.
