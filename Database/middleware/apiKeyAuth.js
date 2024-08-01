const { User } = require('../models'); // Doğru dosya yolunu kontrol edin

const apiKeyAuth = async (req, res, next) => {
  const apiKey = req.headers['x-api-key'];
  if (!apiKey) {
    return res.status(401).json({ message: 'API anahtarı gerekli.' });
  }

  const user = await User.findOne({ where: { apiKey } });
  if (!user) {
    return res.status(403).json({ message: 'Geçersiz API anahtarı.' });
  }

  // Kullanıcıyı isteğe ekleyin
  req.user = user;
  next();
};

module.exports = apiKeyAuth;
