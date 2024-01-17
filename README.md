# RWA_project_Movies
Web App made in ASP.NET using MVC

# Integration Module (ASP.NET Core Web API)
## Overview
The Integration Module uses ASP.NET Core Web API to handle various features related to video content and user management. Here's an overview of the tasks and outcomes:

## Video Content Management (CRUD Operations)
Create RESTful endpoints for retrieving and editing all video content.  
Properties include name, description, image, total time, streaming URL, genre, and tags.  
Implement paging, filtering by name, and sorting by id, name, or total time.  
Endpoint URL: api/videos  
## Genre and Tag Management (CRUD Operations)
Create RESTful endpoints for managing genres and tags.  
Genre properties: name and description.  
Tag property: name.  
Endpoint URLs: api/genres and api/tags  
## Authentication and Security
Implement JWT token-based authentication with user registration and email validation.  
Include a notification with a URL for users to validate their email during registration.  
Secure the video content endpoint.  
Endpoint URL for user management: api/users  
### Notification Management (CRUD Operations)
Create a RESTful endpoint for managing notifications.  
Notification properties: receiver email, subject, and body.  
Include an action to send all pending notifications via SMTP client with configurable settings.  
Endpoint URL: api/notifications  
### Static Web Page
Develop a static web page with the ability to:  
Retrieve the count of pending notifications (add the necessary endpoint).  
Trigger an action to send notifications.  
The page should store and display notification data from local storage.  
# Administrative Module (ASP.NET Core MVC)
## Overview
The Administrative Module, built using ASP.NET Core MVC, focuses on administrative tasks related to video content, country management, and user management.

## Video Content Management (CRUD Operations)  
Create pages for managing video content (CRUD operations).  
The list page should include paging and filtering by video name and genre name.  
Include a button for adding video content that leads to a page for adding video properties.  
Provide a page to view and edit individual video content details, including deletion.  
## Country Management  
Create a page for displaying and managing countries.  
The list of countries should support paging.  
Tag Management (CRUD Operations)  
### User Management  
Create a user list page with filtering by first name, last name, username, and country of origin.  
Remember client-side filters.  
Provide a button for adding users, leading to a user registration page.  
Include the ability to edit user data and deactivate users (soft-delete).  
### Data Mapping  
Implement data mapping between the data access layer and business layer for this module.  
# Public Module (ASP.NET Core MVC)  
## Overview  
The Public Module, also built using ASP.NET Core MVC, serves public-facing pages for user registration, login, video selection, and playback.  

## User Registration  
Create a page for independent user registration without email verification or password change.  
### User Login  
Develop a user login page.  
After successful login, redirect users to the video selection page.  
### Video Selection
Create a page for selecting videos.  
Display video cards with titles, images, and descriptions.  
Implement filtering by video name.  
Clicking on a card takes the user to the video details page.  
### Video Details  
Create a page for displaying detailed video information.  
Include a button for starting video playback that redirects to the video's streaming URL.  
### User Profile  
Show the logged-in user's username on each page.  
Implement the ability to log out.  
### Email Verification for User Registration  
Add email verification functionality to the user registration process.  
### User Profile Page  
Create a user profile page displaying essential user data and providing a link to change the password.  
### Video Pagination  
Implement pagination for video selection using AJAX techniques.  
### Data Mapping  
Implement data mapping between the data access layer and business layer for this module.  
# Conclusion  
This project encompasses an Integration Module, an Administrative Module, and a Public Module, each developed using ASP.NET Core technologies. The Integration Module handles core features like video content management and authentication. The Administrative Module focuses on administrative tasks, while the Public Module serves public-facing pages for user registration, login, video selection, and playback.
