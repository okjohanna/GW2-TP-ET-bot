# GW2 Cat Bot

A Discord bot built in C# with DSharpPlus, combining a few things I genuinely enjoy: cats, cheeky quotes and Guild Wars 2.\
My first C# project.

## Features

- **Slash commands** with clean Discord embed responses
- A **custom local API** (FastAPI + Uvicorn, Python) that the bot queries for cat pictures and quotes, backed by a SQLite database
- **Guild Wars 2 Trading Post integration** - live buy/sell prices for any item via the official GW2 API
- **Custom event timer** for GW2 group events, driven by a JSON event table

______________

## Screenshots

<details>
<summary>Cat pictures!</summary>
<br>
<img width="403" height="381" alt="image" src="https://github.com/user-attachments/assets/8f6562bf-e0a6-412a-940c-851433a114e7" /> <img width="320" height="376" alt="image" src="https://github.com/user-attachments/assets/f441d496-e8c4-4cf1-8f06-a8c164f9a07e" />
</details>

<details>
<summary>Guild Wars 2 stuff!</summary>
<br>
<img width="310" height="184" alt="image" src="https://github.com/user-attachments/assets/d0238f6d-7c1e-4ee4-8af1-2682a351763f" />
<br>
<img width="417" height="298" alt="image" src="https://github.com/user-attachments/assets/5368945d-1af6-4bba-b203-6552ba9a6a85" />
<br>
<img width="425" height="674" alt="image" src="https://github.com/user-attachments/assets/fe30feb8-f7b7-4a57-bc5a-d4ad38b94a79" /> 
</details>

______________

## Commands

| Command | Description |
|---------|-------------|
| `/quote` | Sends a random quote from the local API |
| `/cat` | Sends a random cat picture from the local API |
| `/et` | Lists all upcoming tracked GW2 group events |
| `/et30` | Shows GW2 group events starting in the next 30 minutes |
| `/tp <item_id>` | Shows current Trading Post buy/sell prices for an item |
| `/ping` | Test command - responds with "Pong!" |

## Architecture

The bot itself is a C# / DSharpPlus Discord client. Cat images and quotes are served from a small self-hosted API (FastAPI, Uvicorn) backed by SQLite, run locally alongside the bot. GW2 Trading Post data is fetched live from the official GW2 API. Event data is defined in a JSON table and checked against the current time for the `/et` and `/et30` commands.

## Built with

C#, DSharpPlus, Python, FastAPI, Uvicorn, SQLite, GW2 API

## IDE

Visual Studio
