import puppeteer from "puppeteer-extra";
import StealthPlugin from "puppeteer-extra-plugin-stealth";
import { Browser, ElementHandle, executablePath } from "puppeteer";
import { Region } from "../model/Region";
import { Team } from "../model/Team";

puppeteer.use(StealthPlugin());

const regionScraper = async (): Promise<Region[]> =>  {
  //Portal to a list of regions with teams
  //And the HTML element containing their name and URL
  const scrapeURL: string = "https://liquipedia.net/dota2/Portal:Teams";
  const selector: string = '.panel-box-heading';

  const browser: Browser = await puppeteer.launch({
    headless: true,
    executablePath: executablePath(),
  });

  let region: Region[] = [];
  let teams: Team[] = [];

  try {
    const page = await browser.newPage();
    await page.goto(scrapeURL);

    const regionElements: ElementHandle<Element>[] = await page.$$(selector);

    for (const regionElement of regionElements) {
      const name = await regionElement.$eval("a",(el) => el.textContent?.trim() || "");
      const url = await regionElement.$eval("a",(el) => el.getAttribute("href") || "");

      if(url.includes("Inactive")) {
        continue
      }
      region.push({ name, url });
    }

    console.log(region);
    return region;
  } catch (error) {
    console.error("Error during region scraping:", error);
    return [];
  } finally {
    await browser.close();
  }
};

export default regionScraper;
