module.exports = class CommandResultParser {
    constructor() { };

    parse(data) {
        const result = JSON.parse(data);
        switch (result.Type) {
            case 0: //metrics;
                break;
            case 1: //portfolio;
                break;
            case 2: //list;
                return this._parseList(result.Data.List);
            case 3: //text;
                break;
            case 4: //error;
                break;
        }
    }

    _parseList(list) {
        var string = "Available exchanges:";
        for (let entry in list) {
            string += `\n${list[entry].Text}`
        }
        return string;
    }
}
