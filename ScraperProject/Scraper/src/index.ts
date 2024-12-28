import express, { Request, Response } from "express";
import playerScraper from "./Controller/PlayerScraper";

const app: express.Application = express();
const port = 3000;

app.use(express.json());

app.post("/scrape", async (req: Request, res: Response): Promise<void> => {
    const { url } = req.body;
    if (!url) {
        res.status(400).send("URL is required");
        return;
    }
    try {
        const playerData = await playerScraper(url);
        res.json(playerData);
    } catch (error) {
        console.error(error);
        res.status(500).send("Scraping failed");
    }
});

app.listen(port, () => {
    console.log(`Scraper API is running on http://localhost:${port}`);
});
