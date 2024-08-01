const { User } = require('../models');

const getAllUsers = async (req, res) => {
  try {
    const users = await User.findAll();
    res.status(200).json(users);
  } catch (error) {
    res.status(500).json({ message: 'Kullanıcıları getirirken hata oluştu.', error });
  }
};

// Belirli bir kullanıcıyı ID'si ile getirme fonksiyonu
const getUserById = async (req, res) => {
  const { id } = req.params;
  try {
    // API anahtarına sahip kullanıcı ile istenen kullanıcıyı karşılaştır
    if (req.user.id !== parseInt(id, 10)) {
      return res.status(403).json({ message: 'Bu kullanıcıya erişim izniniz yok.' });
    }

    const user = await User.findByPk(id);
    if (user) {
      res.status(200).json(user);
    } else {
      res.status(404).json({ message: 'Kullanıcı bulunamadı.' });
    }
  } catch (error) {
    res.status(500).json({ message: 'Kullanıcıyı getirirken hata oluştu.', error });
  }
};

module.exports = {
  getAllUsers,
  getUserById
};
