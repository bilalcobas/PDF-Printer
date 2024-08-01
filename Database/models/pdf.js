const { Model, DataTypes } = require('sequelize');

module.exports = (sequelize, DataTypes) => {
  class Pdf extends Model {
    static associate(models) {
      Pdf.belongsTo(models.User, { foreignKey: 'userId', as: 'user' });
    }
  }

  Pdf.init(
    {
      id: {
        type: DataTypes.INTEGER,
        primaryKey: true,
        autoIncrement: true
      },
      userId: {
        type: DataTypes.INTEGER,
        allowNull: false,
        references: {
          model: 'Users',
          key: 'id'
        }
      },
      filePath: {
        type: DataTypes.STRING,
        allowNull: false
      },
      pages: {
        type: DataTypes.INTEGER,
        allowNull: false,
        validate: {
          min: 1
        }
      },
      orientation: {
        type: DataTypes.STRING,
        allowNull: false,
        validate: {
          isIn: [['portrait', 'landscape']]
        }
      }
    },
    {
      sequelize,
      modelName: 'Pdf',
      tableName: 'Pdfs',
      timestamps: true,
      paranoid: true
    }
  );

  return Pdf;
};
