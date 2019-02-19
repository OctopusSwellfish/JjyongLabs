module.exports = (sequelize, DataTypes) => {
    return sequelize.define('test', {
     x: {
         type: DataTypes.DOUBLE,
     },
     y: {
         type: DataTypes.DOUBLE,
     },
     z: {
         type: DataTypes.DOUBLE,
     },
     teststring: {
         type: DataTypes.STRING(100),
     },
    }, {
        timeStamps: true,
    });
};