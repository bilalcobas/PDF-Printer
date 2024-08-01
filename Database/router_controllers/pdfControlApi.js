const { Pdf } = require('../models');

// Tüm PDFleri getiren rota
const getAllPdfs = async (req, res) => {
  try {
    const pdfs = await Pdf.findAll();
    res.status(200).json(pdfs);
  } catch (error) {
    res.status(500).json({ message: 'PDFleri getirirken hata oluştu.', error });
  }
};

// Belirli bir PDF'i ID'si ile getiren rota
const getPdfById = async (req, res) => {
  const { id } = req.params;
  try {
    const pdf = await Pdf.findByPk(id);
    if (pdf) {
      res.status(200).json(pdf);
    } else {
      res.status(404).json({ message: 'PDF bulunamadı.' });
    }
  } catch (error) {
    res.status(500).json({ message: 'PDF\'yi getirirken hata oluştu.', error });
  }
};

// Belirli bir PDF'in filePath'ini getiren rota
const getPdfFilePathById = async (req, res) => {
  const { id } = req.params;
  try {
    const pdf = await Pdf.findByPk(id);
    if (pdf) {
      res.status(200).json({ filePath: pdf.filePath });
    } else {
      res.status(404).json({ message: 'PDF bulunamadı.' });
    }
  } catch (error) {
    res.status(500).json({ message: 'PDF\'nin filePath\'ini getirirken hata oluştu.', error });
  }
};

module.exports = {
  getAllPdfs,
  getPdfById,
  getPdfFilePathById
};
