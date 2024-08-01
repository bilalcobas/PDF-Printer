const { Pdf } = require('../models');

const getAllPdfs = async () => {
  return await Pdf.findAll();
};

const createPdf = async (pdfData) => {
  return await Pdf.create(pdfData);
};

module.exports = {
  getAllPdfs,
  createPdf
};
