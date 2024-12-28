import playerScraper from "../Controller/PlayerScraper";

export type Player = {
    inGameName: string;
    name: string;
    nationality: string;
    earnings: number;
    dateOfBirth: string;
    region: string;
    yearsActive: string;
    currentRoles: string[];
    team: string;
    url: string;
}

export const showPlayer = (player: Player): void => {
    Object.keys(player).map(o => console.log(o));
};