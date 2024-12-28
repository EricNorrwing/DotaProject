import { Page } from "puppeteer";
import { PlayerExtractor } from "./PlayerExtractor"; // Assuming this is now an object
import { Player } from "../../model/Player";

const playerInfoBoxExtractor = async (
    page: Page,
    partialPlayer: Player
): Promise<Player> => {
    const generalDataSelector = PlayerExtractor.generalDataExtractor;

    return await page.evaluate((generalDataSelector, partialPlayer) => {
        const infoBoxLabels: Element[] = Array.from(document.querySelectorAll(generalDataSelector));

        infoBoxLabels.forEach((value) => {
            const extractedHtmlElementValue = value.textContent?.trim();

            switch (extractedHtmlElementValue) {
                case "Name:":
                    partialPlayer.name = value.nextElementSibling?.textContent?.trim() || "";
                    break;
                case "Nationality:":
                    partialPlayer.nationality = value.nextElementSibling?.textContent?.trim() || "";
                    break;
                case "Approx. Total Winnings:":
                    const earningsText = value.nextElementSibling?.textContent?.replace(/[^0-9.]/g, "").trim() || "0";
                    partialPlayer.earnings = Number(earningsText);
                    break;
                case "Born:":
                    partialPlayer.dateOfBirth = value.nextElementSibling?.textContent?.trim() || "";
                    break;
                case "Region:":
                    partialPlayer.region = value.nextElementSibling?.textContent?.trim() || "";
                    break;
                case "Years Active (Player):":
                    partialPlayer.yearsActive = value.nextElementSibling?.textContent?.trim() || "";
                    break;
                case "Current Role:":
                    partialPlayer.currentRoles = value.nextElementSibling?.textContent?.split(",").map((role) => role.trim()) || [];
                    break;
                case "Team:":
                    partialPlayer.team = value.nextElementSibling?.textContent?.trim() || "";
                    break;
                default:
                    break;
            }
        });

        return partialPlayer;
    }, generalDataSelector, partialPlayer);
};

export default playerInfoBoxExtractor;
