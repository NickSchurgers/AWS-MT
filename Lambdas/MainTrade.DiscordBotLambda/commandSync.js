const path = require('path');
const { SlashCreator } = require('slash-create');

const creator = new SlashCreator({
    applicationID: process.env.DISCORD_APP_ID,
    publicKey: process.env.DISCORD_PUBLIC_KEY,
    token: process.env.DISCORD_BOT_TOKEN,
});

exports.handler = async (event) => {
    creator
        .registerCommandsIn(path.join(__dirname, 'commands'))
        .syncCommands();
};