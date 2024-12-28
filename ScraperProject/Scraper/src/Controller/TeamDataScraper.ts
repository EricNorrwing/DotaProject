import puppeteer from "puppeteer-extra";
import StealthPlugin from "puppeteer-extra-plugin-stealth";
import { Browser, executablePath } from "puppeteer";
import { Player } from "../model/Player";
import { Team } from "../model/Team";
import baseURL from "../baseUrl";

puppeteer.use(StealthPlugin());

const playerScraper = async (teamURL: string, teamName: string): Promise<Team> => {
    const browser: Browser = await puppeteer.launch({
        headless: true,
        executablePath: executablePath(),
    });

    //const scrapeURL: string = baseURL + relTeamURL;
    let team: Team = {
        name: teamName,
        url: teamURL,
        earnings : 0,
        region: "", 
        location: ""   
    };

    try {
        const page = await browser.newPage();
        await page.goto(teamURL);

        const playerInfo = await page.evaluate(() => {
            const labels = Array.from(document.querySelectorAll("div.infobox-cell-2.infobox-description"));

            labels.forEach((labelElement) => {
                const label = labelElement.textContent?.trim();

                switch (label) {
                    case "Location:":
                        team.location = labelElement.nextElementSibling?.textContent?.trim() || "";
                        break;
                    case "Region:":
                        team.region = labelElement.nextElementSibling?.textContent?.trim() || "";
                        break;
                    /*
                    case "Coach:":
                        team.coach = labelElement.nextElementSibling?.textContent?.trim() || "";
                        break;
                    case "Manager":
                        team.manager = labelElement.nextElementSibling?.textContent?.trim() || "";
                        break; */
                    case "Approx. Total Winnings:":
                        const earningsText = labelElement.nextElementSibling?.textContent?.replace(/[^0-9.]/g, "").trim() || "0";
                        team.earnings = Number(earningsText);
                        break;
                    default:
                        break;
                }
            });

            return {
                team
            };
        });
       

        return team;
    } catch (error) {
        console.error("Error during player scraping:", error);
        return team;
    } finally {
        await browser.close();
    }
};

export default playerScraper;
