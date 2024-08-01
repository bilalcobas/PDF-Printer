// seeders/seedData1.js
const { Sequelize } = require('sequelize');
const userService = require('../services/userService');
const pdfService = require('../services/pdfService');
const path = require('path');

const sequelize = new Sequelize('database1', 'root', 'bc123', {
  host: 'localhost',
  dialect: 'mysql'
});

const seedData = async () => {
  try {
    await sequelize.authenticate();
    console.log('Bağlantı başarılı!');

    // Modelleri senkronize et
    await sequelize.sync({ force: false });

    // İlk kullanıcı oluşturma
    const user1 = await userService.createUser({
      firstName: 'Ahmet',
      lastName: 'Yılmaz',
      role: 'user'
    });
    console.log('Yeni kullanıcı oluşturuldu:', user1.toJSON());

    const user2 = await userService.createUser({
      firstName: 'Mehmet',
      lastName: 'Yıldız',
      role: 'user'
    });
    console.log('Yeni kullanıcı oluşturuldu:', user2.toJSON());

    const user3 = await userService.createUser({
      firstName: 'Ali',
      lastName: 'Vefa',
      role: 'user'
    });
    console.log('Yeni kullanıcı oluşturuldu:', user3.toJSON());

    // İlk kullanıcı için PDF oluşturma
    const pdf1 = await pdfService.createPdf({
      userId: user1.id,
      filePath: path.join(__dirname, '..', 'uploads', '1.pdf'),
      pages: 12,
      orientation: 'landscape'
    });
    console.log('Yeni PDF oluşturuldu:', pdf1.toJSON());

    const pdf2 = await pdfService.createPdf({
      userId: user1.id,
      filePath: path.join(__dirname, '..', 'uploads', '1.pdf'),
      pages: 10,
      orientation: 'landscape'
    });
    console.log('Yeni PDF oluşturuldu:', pdf2.toJSON());

    const pdf3 = await pdfService.createPdf({
      userId: user1.id,
      filePath: path.join(__dirname, '..', 'uploads', '1.pdf'),
      pages: 10,
      orientation: 'landscape'
    });
    console.log('Yeni PDF oluşturuldu:', pdf3.toJSON());

    const pdf4 = await pdfService.createPdf({
      userId: user1.id,
      filePath: path.join(__dirname, '..', 'uploads', '1.pdf'),
      pages: 8,
      orientation: 'landscape'
    });
    console.log('Yeni PDF oluşturuldu:', pdf4.toJSON());

    const pdf5 = await pdfService.createPdf({
      userId: user1.id,
      filePath: path.join(__dirname, '..', 'uploads', '1.pdf'),
      pages: 4,
      orientation: 'landscape'
    });
    console.log('Yeni PDF oluşturuldu:', pdf5.toJSON());

    const pdf6 = await pdfService.createPdf({
      userId: user1.id,
      filePath: path.join(__dirname, '..', 'uploads', '1.pdf'),
      pages: 6,
      orientation: 'landscape'
    });
    console.log('Yeni PDF oluşturuldu:', pdf6.toJSON());

    const pdf7 = await pdfService.createPdf({
      userId: user1.id,
      filePath: path.join(__dirname, '..', 'uploads', '1.pdf'),
      pages: 1,
      orientation: 'landscape'
    });
    console.log('Yeni PDF oluşturuldu:', pdf7.toJSON());

    const pdf8 = await pdfService.createPdf({
      userId: user1.id,
      filePath: path.join(__dirname, '..', 'uploads', '1.pdf'),
      pages: 2,
      orientation: 'landscape'
    });
    console.log('Yeni PDF oluşturuldu:', pdf8.toJSON());

    const pdf9 = await pdfService.createPdf({
      userId: user1.id,
      filePath: path.join(__dirname, '..', 'uploads', '1.pdf'),
      pages: 3,
      orientation: 'landscape'
    });
    console.log('Yeni PDF oluşturuldu:', pdf9.toJSON());

    const pdf10 = await pdfService.createPdf({
      userId: user1.id,
      filePath: path.join(__dirname, '..', 'uploads', '1.pdf'),
      pages: 10,
      orientation: 'landscape'
    });
    console.log('Yeni PDF oluşturuldu:', pdf10.toJSON());

  } catch (error) {
    console.error('Bağlantı hatası:', error);
  } finally {
    await sequelize.close();
  }
};

seedData();
