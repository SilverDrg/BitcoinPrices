# BitcoinPrices

This is a .Net application that fetches the current price of Bitcoin every hour and saves it to an SQLite database.
On startup if the database is empty it gets seeded with random price data equivalent to 14 days of running.

It consists of 4 endpoints:
- GetCurrentPrice - Returns the latest price in the database
- GetAveragePriceForDate/{date} - Takes a DateTime value and returns the average price for the given date
- All - Returns all values stored in the database
- RemoveAll - Removes all values store in the database
