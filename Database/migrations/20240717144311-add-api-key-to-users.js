'use strict';

module.exports = {
  up: async (queryInterface, Sequelize) => {
    await queryInterface.addColumn('Users', 'apiKey', {
      type: Sequelize.STRING,
      allowNull: false,
      defaultValue: () => require('crypto').randomBytes(16).toString('hex')
    });
  },

  down: async (queryInterface, Sequelize) => {
    await queryInterface.removeColumn('Users', 'apiKey');
  }
};
