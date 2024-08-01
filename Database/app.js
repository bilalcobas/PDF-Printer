const express = require('express');
const { Sequelize } = require('sequelize');
const apiRouter = require('./api');

const app = express();
const port = 3001;

const sequelize = new Sequelize('database1', 'root', 'bc123', {
  host: 'localhost',
  dialect: 'mysql'
});

const User = require('./models/user')(sequelize, Sequelize.DataTypes);
const Pdf = require('./models/pdf')(sequelize, Sequelize.DataTypes);

const onConnect = async () => {
  try {
    await sequelize.authenticate();
    console.log('Bağlantı başarılı!');0
    
    // Kullanıcı tablosunu senkronize et
    await User.sync({ force: false });
    
    // Pdf tablosunu senkronize et
    await Pdf.sync({ force: false });
    
  } catch (error) {
    console.error('Bağlantı hatası:', error);
  }
};

// Middleware'ler
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

onConnect();


// API rotalarını kullan
app.use('/api', apiRouter);

// Diğer ayarlar ve dinleyici...
const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Server is running on port ${PORT}`);
});

app.listen(port, () => {
  console.log(`Uygulama http://localhost:${port} adresinde çalışıyor`);
});
