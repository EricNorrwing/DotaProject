import puppeteer from 'puppeteer-extra';
import StealthPlugin from 'puppeteer-extra-plugin-stealth';
import { Browser, ElementHandle, executablePath } from 'puppeteer';
import baseURL from '../baseUrl';
import { Team } from '../model/Team';
import TeamDataScraper from "./TeamDataScraper"
import playerScraper from './PlayerScraper';
import { Player } from '../model/Player';

puppeteer.use(StealthPlugin());

const teamScraper = async (relRegionURL: string): Promise<Team[]> => {
  
    const browser: Browser = await puppeteer.launch({
        headless: true,
        executablePath: executablePath(),
      });

    const regionURL: string = baseURL()+relRegionURL
    console.log(regionURL)
    const selector: string = '.team-template-text'
    const teams: Team[] = []

    try {
        const page = await browser.newPage();
        await page.goto(regionURL);

        const teamElements: ElementHandle<Element>[] = await page.$$(selector);

        for (const teamElement of teamElements) {
            const name = await teamElement.$eval("a",(el) => el.textContent?.trim() || "")
            const url = await teamElement.$eval("a",(el) => el.getAttribute("href") || "")

            if(url) {
              const teamPage = await browser.newPage();
              await teamPage.goto(baseURL() + url)

              const team: Team = await TeamDataScraper(url, name)
              console.log(team)
              



            }

            //const players: Player[] = playerScraper(url)
            console.log(name, url)
            //teams.push({ name, url, players, staff, region, earnings, location, active});
          }


    return teams
  } catch (error) {
    console.error('Error during team scraping:', error)
    return []
  } finally {
    await browser.close();
  }
};

export default teamScraper
