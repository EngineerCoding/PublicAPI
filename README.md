# Public API

This repository contains the backend which depicts (mostly) a publicly API. This also includes some restricted endpoints, but those are only related to the functionality and didn't make sence to put into a dedicated API.

Available endpoints

## 1. Opengraph

Endpoints which can be used to retrieve opengraph enabled pages and refresh the internal cache. This is a fixed cache, which means that it has to be updated before new posts appear.

## GET `/opengraph/[slug]`

Generates a HTML file which contains [OpenGraph](https://ogp.me/) tags so that my blog posts can be previewed in social media. This page contains the OG tags and in the body it directly attempts to redirect the user to the blog post hosted on `https://ameling.dev/blog.html#[slug]`. Because JavaScript is not enabled by definition, although it most of the time is, this page also contains an `iframe` showing the blog post full screen. The user navigates to `https://ameling.dev` urls when navigating within this `iframe`.

The url parameter `slug` must be a slug referring to a blog post. A list of blog posts is stored in [blog_posts.json](https://ameling.dev/res/data/blog_posts.json). The title of each entry should then be converted to lowercase, spaces should be replaced by hyphens and finally the result should be URL encoded.

## POST `/opengraph/refresh`

This refreshes the internal cache by fetching the [blog_posts.json](https://ameling.dev/res/data/blog_posts.json) and updating the database with the correct data, such as slugs and author. Note that this endpoint requires a valid bearer token.
