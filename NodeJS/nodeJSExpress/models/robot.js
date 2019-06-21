
module.exports = (sequelize, DataTypes) => {
	return sequelize.define('robot', {
	/*	
		px: {
			type: DataTypes.DOUBLE,
		},
		py: {
			type: DataTypes.DOUBLE,
		},
		pz: {
			type: DataTypes.DOUBLE,
		},
		rx: {
			type: DataTypes.DOUBLE,
		},
		ry: {
			type: DataTypes.DOUBLE,
		},
		rz: {
			type: DataTypes.DOUBLE,
		},
		ax: {
			type: DataTypes.DOUBLE,
		},
		ay: {
			type: DataTypes.DOUBLE,
		},
		az: {
			type: DataTypes.DOUBLE,
		},
	*/
		result: {
			type: DataTypes.STRING,
		},
		gripperValue: {
			type: DataTypes.DOUBLE,
		},
		transform_value_0: {
			type: DataTypes.DOUBLE,
		},
 		transform_value_1: {
                        type: DataTypes.DOUBLE,
                },
		 transform_value_2: {
                        type: DataTypes.DOUBLE,
                },
		 transform_value_3: {
                        type: DataTypes.DOUBLE,
                },
		 transform_value_4: {
                        type: DataTypes.DOUBLE,
                },
		 transform_value_5: {
                        type: DataTypes.DOUBLE,
                },
			
	}, {
		timestamps: true,
});
	
};
