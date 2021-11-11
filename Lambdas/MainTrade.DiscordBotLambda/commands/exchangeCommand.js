const AWS = require('aws-sdk');
const { SlashCommand } = require('slash-create');
const lambda = new AWS.Lambda();
const CommandResultParser = require('../commandResultParser')

module.exports = class ExchangeCommand extends SlashCommand {
    constructor(creator) {
        super(creator, {
            name: 'exchange',
            description: 'List exchanges.',
            options: []
        });

        // Not required initially, but required for reloading with a fresh file.
        this.filePath = __filename;
    }

    async run(ctx) {
        let params = {
            FunctionName: 'CommandProcessor', // the lambda function we are going to invoke
            InvocationType: 'RequestResponse',
            LogType: 'Tail',
            Payload: '["exchange"]'
        };

        const result = await invokeLambda(lambda, params);
        const parser = new CommandResultParser();
        const parsed = parser.parse(result.Payload);

        return parsed;
    }
}

const invokeLambda = (lambda, params) => new Promise((resolve, reject) => {
    lambda.invoke(params, (error, data) => {
        if (error) {
            reject(error);
        } else {
            resolve(data);
        }
    });
});