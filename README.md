# Mood_Feed
Programming Capstone Project/C# Webscraper
![Screenshot 2023-07-14 233358](https://github.com/SBurgerss/Mood_Feed/assets/131003779/2edb0f0d-b8c1-47ab-861e-763f22bd19e0)
\nDescription:
This is a C#/HtmlAgility pack based webscraper built upon the Visual Studio 2022 WPF application platform.
\nPurpose:
The purpose of this project is to create an application where news content is filtered and hidden 
behind three buttons. These buttons will be based off of three moods for reading content; a 
“positive” emoji representing news articles with wholesome or victorious headlines, a “morbidly 
curious” emoji for news articles that feature intimidating headlines or negative content, and a 
“random” emoji that will present articles that can’t be filtered in either the first 2 emoji moods or 
are neutral in nature without any emotionally charged headlines. This application will be able to 
hide all 3 types of filtered content behind UI, therefore eliminating any chances of a user being 
turned off or triggered by content they had no intent of being exposed to. This UI will be able to 
let the user take back control of their content and what content they want to browse.
\nImplementation:
Upon clicking upon a "reading mood"-based emoji button, a custom method for parsing x path values from a specified website is triggered. This method utilizes html agility pack Web instances and then the desired web page data is extracted from the web page API, allowing the program to convert the data into a readable string-which will be formatted into a listbox so the user can scroll through scraped article results.
This program features a connected access database as well, if the user desires to save an article result for later reading, they can click on a bookmark button; This bookmark button will then store the article information in the connected database for later reading.
When the user wants to access saved articles, there is a 'safe' icon that when clicked, launches a 'saved articles' window that displays all of the database/saved article data. The saved articles window also allows the user to permanently delete selected articles from the database for the use case of no longer wanting the article to be saved or stored.
\nClosing:
Any comments or constructive criticism on this is greatly encouraged.

