# net-sendgrid-email-client

Tool for sending emails via any address from a domain you control using SendGrid's API.

## The Problem

You may have a registered domain, want to use multiple email addresses associated with that domain, and have them all forward to one address.

You can set up a catch-all address with your domain registrar, which takes care of the receiving aspect.

But in my experience, there's not a simple way to send with any address on your domain.

## The Solution

When your domain is authenticated with SendGrid, you can send emails with any address on your domain using SendGrid's email API.

[app.sendgrid.com](https://app.sendgrid.com/) does not have a UI, however. So we need one.

This application is a simple .NET MVC web app that serves this purpose.

## Authentication

If you only care about being able to do this from you local machine, you could just pull down the source and run the app locally in Visual Studio.

However, I wanted to be able to send emails from any device. For this we can deploy to an Azure app service. There's a free tier (with quotas) that meets my needs.

But, of course, I only want authorized users (namely me) to be able to send emails using my email domain.

Authentication is hard, so I'm using [Google Open ID Connect](https://developers.google.com/identity/protocols/oauth2/openid-connect) to authenticate.

You will need to create an OAuth client ID in Google Cloud, and add the client ID and secret to the app configuration.

You will also need to configure an OAuth consent screen.

## Authorization

You may be tempted to use Google Cloud for authorization, leaving the app with a publishing status of "Testing" and whitelisting your users there. That doesn't work (unless Google fixes it in the future). Google currently does not limit access to the whitelisted test users.

Instead, use .NET's policy/claims-based auth. Only users with email addresses whitelisted in the admins section of the app configuration will be able to access the email functionality.

