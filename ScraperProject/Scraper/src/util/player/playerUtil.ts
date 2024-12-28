

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

export const updatePlayer = (player: Player, partialPlayer: Player): Player => {

    if (partialPlayer.inGameName !== undefined) {
        player.inGameName = partialPlayer.inGameName;
    }
    if (partialPlayer.name !== undefined) {
        player.name = partialPlayer.name;
    }
    if (partialPlayer.nationality !== undefined) {
        player.nationality = partialPlayer.nationality;
    }
    if (partialPlayer.earnings !== undefined) {
        player.earnings = partialPlayer.earnings;
    }
    if (partialPlayer.dateOfBirth !== undefined) {
        player.dateOfBirth = partialPlayer.dateOfBirth;
    }
    if (partialPlayer.region !== undefined) {
        player.region = partialPlayer.region;
    }
    if (partialPlayer.yearsActive !== undefined) {
        player.yearsActive = partialPlayer.yearsActive;
    }
    if (partialPlayer.currentRoles !== undefined) {
        player.currentRoles = partialPlayer.currentRoles;
    }
    if (partialPlayer.team !== undefined) {
        player.team = partialPlayer.team;
    }
    if (partialPlayer.url !== undefined) {
        player.url = partialPlayer.url;
    }

    return player;
};

export const showPlayer = (player: Player): void => {
    Object.keys(player).forEach(o => console.log(`${o}: ${player[o as keyof Player]}`));
};
