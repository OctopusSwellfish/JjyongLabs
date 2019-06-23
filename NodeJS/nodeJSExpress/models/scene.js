module.exports = (sequelize, DataTypes) => {
	return sequelize.define('scene', {
		step: {
			type: DataTypes.INTEGER
		},
	},{
		timestamps: true,
});
};
