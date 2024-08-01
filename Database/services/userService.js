const { User, Pdf } = require('../models');

const getAllUsersWithPdfs = async () => {
  return await User.findAll({
    include: [
      {
        model: Pdf,
        as: 'pdfs'
      }
    ]
  });
};

const createUser = async (userData) => {
  return await User.create(userData);
};

module.exports = {
  getAllUsersWithPdfs,
  createUser
};
