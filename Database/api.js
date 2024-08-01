const express = require('express');
const userControl = require('./router_controllers/userControl.js');
const pdfControl = require('./router_controllers/pdfControl.js');
const userControllerApi = require('./router_controllers/userControlApi.js');
const pdfControllerApi = require('./router_controllers/pdfControlApi.js');
const apiKeyAuth = require('./middleware/apiKeyAuth');

const router = express.Router();

// Kullanıcıları ve PDF bilgilerini döndüren endpoint
router.get('/users', async (req, res) => { 
  try {
    const users = await userControl.getAllUsersWithPdfs();
    res.status(200).json(users);
  } catch (error) {
    console.error('veri çekme hatası:', error);
    res.status(500).json({ message: 'Veri çekme işlemi başarısız oldu.', error: error.message });
  }
});

// PDF'leri döndüren endpoint
router.get('/pdfs', async (req, res) => {
  try {
    const pdfs = await pdfControl.getAllPdfs();
    res.status(200).json(pdfs);
  } catch (error) {
    console.error('PDF veri çekme hatası:', error);
    res.status(500).json({ message: 'PDF veri çekme işlemi başarısız oldu.', error: error.message });
  }
});

// Belirli bir kullanıcının tüm PDF'lerini getiren rota
router.get('/users/:id/pdfs', apiKeyAuth, async (req, res) => {
  const { id } = req.params;
  try {
    // Kullanıcıya ait tüm PDF'leri al
    const user = await userControl.getAllUsersWithPdfs();
    const userWithPdfs = user.find(u => u.id === parseInt(id, 10));

    if (userWithPdfs) {
      res.status(200).json(userWithPdfs.pdfs);
    } else {
      res.status(404).json({ message: 'Kullanıcı bulunamadı.' });
    }
  } catch (error) {
    console.error('PDF veri çekme hatası:', error);
    res.status(500).json({ message: 'PDF veri çekme işlemi başarısız oldu.', error: error.message });
  }
});

// Tüm kullanıcıları getiren api çağrısı
router.get('/users', apiKeyAuth, userControllerApi.getAllUsers);

// Belirli bir kullanıcıyı ID'si ile getiren api çağrısı
router.get('/users/:id', apiKeyAuth, userControllerApi.getUserById);

// Belirli bir PDF'i ID'si ile getiren api çağrısı
router.get('/pdfs/:id', apiKeyAuth, pdfControllerApi.getPdfById);

// Belirli bir PDF'in filePath'ini getiren api çağrısı
router.get('/pdfs/:id/path', apiKeyAuth, pdfControllerApi.getPdfFilePathById);

module.exports = router;
