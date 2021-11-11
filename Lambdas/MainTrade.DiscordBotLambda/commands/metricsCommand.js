const { SlashCommand, CommandOptionType } = require('slash-create');

module.exports = class MetricsCommand extends SlashCommand {
    constructor(creator) {
        super(creator, {
            name: 'metrics',
            description: 'Says hello to you.',
            options: [{
                type: CommandOptionType.STRING,
                name: 'pair',
                description: 'The pair to get the metrics for',
                required: true,
                autocomplete: true
            }]
        });

        // Not required initially, but required for reloading with a fresh file.
        this.filePath = __filename;
    }

    async autocomplete(ctx) {
        // You can send a list of choices with `ctx.sendResults` or by returning a list of choices.
        // Get the focused option name with `ctx.focused`.
        return [{ name: `Your text: ${ctx.options[ctx.focused]}`, value: ctx.options[ctx.focused] }];
    }

    async run(ctx) {
        return `> ${ctx.options.greeting}\nHello!`;
    }
}