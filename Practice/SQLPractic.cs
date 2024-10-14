create table employee (
    id uuid primary key ,
    name varchar(50) not null ,
    surname varchar(50) not null ,
    phone_number varchar(15) not null unique ,
    passport varchar(50) not null unique ,
    address varchar(100) not null ,
    date date not null ,
    bonus integer ,
    position varchar(50) not null ,
    salary numeric not null ,
    date_start_work date not null ,
    contract varchar(100)
);

create table client (
    id uuid primary key ,
    name varchar(50) not null ,
    surname varchar(50) not null ,
    phone_number varchar(15) not null unique ,
    passport varchar(50) not null unique ,
    address varchar(100) not null ,
    date date not null ,
    bonus integer
);

create table  account (
    id uuid primary key ,
    currency_name varchar(20) not null ,
    amount numeric ,
    client_id uuid not null ,
    constraint fk_client
        foreign key (client_id) references client(id) on delete cascade on update cascade
);

insert into employee (id, name, surname, phone_number, passport, address, date, bonus, position, salary, date_start_work)
values
       (uuid_generate_v4(), 'Пётр', 'Иванов', '+37377744543', 'I-ПР 123456789', 'Тирасполь, ул. Комсомольская', '2000-01-01', 1, 'Уборщик', 5000, '2019-01-01'),
       (uuid_generate_v4(), 'Аркадий', 'Паровозов', '+37377787451', 'I-ПР 912345678', 'Тирасполь, ул. Юности', '1985-05-07', 1, 'Бухгалтер', 5500, '2019-08-01'),
       (uuid_generate_v4(), 'Алексей', 'Петров', '+37377712345', 'I-ПР 987654321', 'Бендеры, ул. Ленина', '1985-03-15', 2, 'Охранник', 4000, '2018-05-10'),
       (uuid_generate_v4(), 'Анна', 'Сидорова', '+37377767890', 'I-ПР 112233445', 'Рыбница, ул. Шевченко', '1990-07-25', 3, 'Менеджер', 6000, '2020-02-20'),
       (uuid_generate_v4(), 'Мария', 'Фёдорова', '+37377798765', 'I-ПР 998877665', 'Григориополь, ул. Победы', '1978-11-10', 4, 'Кассир', 4500, '2017-08-01'),
       (uuid_generate_v4(), 'Владимир', 'Кузнецов', '+37377765432', 'I-ПР 556677889', 'Дубоссары, ул. Мира', '1982-05-22', 5, 'Администратор', 7000, '2016-12-10'),
       (uuid_generate_v4(), 'Наталья', 'Морозова', '+37377711223', 'I-ПР 223344556', 'Слободзея, ул. Гагарина', '1992-09-15', 6, 'Бухгалтер', 5500, '2019-11-15'),
       (uuid_generate_v4(), 'Ольга', 'Попова', '+37377733445', 'I-ПР 334455667', 'Тирасполь, ул. Лермонтова', '1980-02-28', 7, 'Секретарь', 3800, '2018-03-20'),
       (uuid_generate_v4(), 'Сергей', 'Ковалёв', '+37377744566', 'I-ПР 889900112', 'Бендеры, ул. Горького', '1995-06-18', 8, 'Программист', 8000, '2021-01-10'),
       (uuid_generate_v4(), 'Ирина', 'Васильева', '+37377755667', 'I-ПР 776655443', 'Рыбница, ул. Чехова', '1987-12-05', 9, 'Аналитик', 6200, '2020-09-05'),
       (uuid_generate_v4(), 'Дмитрий', 'Гордеев', '+37377766778', 'I-ПР 443322110', 'Григориополь, ул. Красных Партизан', '1993-04-14', 10, 'Дизайнер', 5600, '2017-06-25'),
       (uuid_generate_v4(), 'Екатерина', 'Филиппова', '+37377777889', 'I-ПР 778899223', 'Дубоссары, ул. Кирова', '1983-08-02', 11, 'Маркетолог', 6400, '2019-10-17'),
       (uuid_generate_v4(), 'Максим', 'Ермаков', '+37377788990', 'I-ПР 110022334', 'Слободзея, ул. Дружбы', '1991-07-12', 12, 'Инженер', 7200, '2021-03-29'),
       (uuid_generate_v4(), 'Алина', 'Захарова', '+37377799001', 'I-ПР 554433667', 'Тирасполь, ул. Краснодонская', '1979-05-30', 13, 'HR-менеджер', 5000, '2020-12-10'),
       (uuid_generate_v4(), 'Виктор', 'Мельников', '+37377700112', 'I-ПР 667788990', 'Бендеры, ул. Котовского', '1988-10-07', 14, 'Адвокат', 9000, '2018-07-23'),
       (uuid_generate_v4(), 'Светлана', 'Смирнова', '+37377711233', 'I-ПР 998877554', 'Рыбница, ул. Розы Люксембург', '1996-11-19', 15, 'Экономист', 6600, '2021-08-11');

insert into client (id, name, surname, phone_number, passport, address, date, bonus)
values
       (uuid_generate_v4(), 'Андрей', 'Семенов', '+37377722345', 'I-ПР 654321789', 'Тирасполь, ул. Победы', '1990-03-10', 200),
       (uuid_generate_v4(), 'Елена', 'Михайлова', '+37377733456', 'I-ПР 123456780', 'Бендеры, ул. Московская', '1988-07-22', 300),
       (uuid_generate_v4(), 'Олег', 'Новиков', '+37377744567', 'I-ПР 098765432', 'Рыбница, ул. Ленина', '1985-04-15', 250),
       (uuid_generate_v4(), 'Виктория', 'Лебедева', '+37377755678', 'I-ПР 567890123', 'Григориополь, ул. Гагарина', '1995-05-28', 180),
       (uuid_generate_v4(), 'Иван', 'Киселев', '+37377766789', 'I-ПР 234567890', 'Дубоссары, ул. Победы', '1992-09-12', 300),
       (uuid_generate_v4(), 'Наталия', 'Зайцева', '+37377777890', 'I-ПР 876543210', 'Слободзея, ул. Ленина', '1982-11-01', 400),
       (uuid_generate_v4(), 'Александр', 'Павлов', '+37377788901', 'I-ПР 345678912', 'Тирасполь, ул. Комсомольская', '1993-01-23', 320),
       (uuid_generate_v4(), 'Марина', 'Сорокина', '+37377799012', 'I-ПР 456789123', 'Бендеры, ул. Горького', '1989-03-19', 270),
       (uuid_generate_v4(), 'Сергей', 'Морозов', '+37377700123', 'I-ПР 654321987', 'Рыбница, ул. Шевченко', '1991-12-05', 310),
       (uuid_generate_v4(), 'Дарья', 'Калинина', '+37377711234', 'I-ПР 234567891', 'Григориополь, ул. Мира', '1994-06-18', 190),
       (uuid_generate_v4(), 'Дмитрий', 'Голубев', '+37377722346', 'I-ПР 112233445', 'Дубоссары, ул. Дружбы', '1987-02-07', 280),
       (uuid_generate_v4(), 'Ксения', 'Волкова', '+37377733457', 'I-ПР 556677889', 'Слободзея, ул. Победы', '1996-10-23', 150),
       (uuid_generate_v4(), 'Михаил', 'Кудрявцев', '+37377744568', 'I-ПР 334455667', 'Тирасполь, ул. Котовского', '1990-09-12', 210),
       (uuid_generate_v4(), 'Юлия', 'Герасимова', '+37377755679', 'I-ПР 667788990', 'Бендеры, ул. Московская', '1985-05-08', 320),
       (uuid_generate_v4(), 'Артур', 'Никифоров', '+37377766790', 'I-ПР 998877665', 'Рыбница, ул. Титова', '1988-01-19', 270),
       (uuid_generate_v4(), 'Екатерина', 'Соколова', '+37377777891', 'I-ПР 223344556', 'Григориополь, ул. Мира', '1997-07-03', 200);

insert into account (id, currency_name, amount, client_id)
values
       (uuid_generate_v4(),'Доллар США', 1000, '9744d4d3-aef0-44eb-bbf5-75d5d786f511'),
       (uuid_generate_v4(), 'Евро', 500, '9744d4d3-aef0-44eb-bbf5-75d5d786f511'),
       (uuid_generate_v4(), 'Доллар США', 1500, '4bf6426f-d373-4a41-b570-50fd5bad79c7'),
       (uuid_generate_v4(), 'Рубль РФ', 30000, '4bf6426f-d373-4a41-b570-50fd5bad79c7'),
       (uuid_generate_v4(), 'Доллар США', 2000, '6ec70fb8-bc03-4c5a-be10-04b35a0e73bf'),
       (uuid_generate_v4(), 'Евро', 800, '6ec70fb8-bc03-4c5a-be10-04b35a0e73bf'),
       (uuid_generate_v4(), 'Рубль РФ', 40000, '6ec70fb8-bc03-4c5a-be10-04b35a0e73bf'),
       (uuid_generate_v4(), 'Доллар США', 1200, 'b091133e-8590-440d-b787-a99bffa21277'),
       (uuid_generate_v4(), 'Доллар США', 1700, '27729929-1a59-4f3b-a879-f0965bead7c5'),
       (uuid_generate_v4(), 'Евро', 900, '27729929-1a59-4f3b-a879-f0965bead7c5'),
       (uuid_generate_v4(), 'Доллар США', 1100, 'f69362ff-c589-44a1-afef-f9f570d43e09'),
       (uuid_generate_v4(), 'Доллар США', 1300, '4894e3c3-2d10-4fd2-af00-221b7905e8af'),
       (uuid_generate_v4(), 'Евро', 600, '4894e3c3-2d10-4fd2-af00-221b7905e8af'),
       (uuid_generate_v4(), 'Доллар США', 800, 'd4c18b20-14e5-48f2-84ff-9a9c073e0a32'),
       (uuid_generate_v4(), 'Доллар США', 1400, '275dc909-3256-4a20-a8d2-85e74e25cff2'),
       (uuid_generate_v4(), 'Евро', 450, '275dc909-3256-4a20-a8d2-85e74e25cff2'),
       (uuid_generate_v4(), 'Доллар США', 1600, '629b9943-75a0-4ba4-b9d5-fb7e7882dea9'),
       (uuid_generate_v4(), 'Рубль РФ', 20000, '629b9943-75a0-4ba4-b9d5-fb7e7882dea9'),
       (uuid_generate_v4(), 'Доллар США', 900, 'fb887e9f-32de-47d4-8253-f7f3933eecb6'),
       (uuid_generate_v4(), 'Доллар США', 1100, 'ed3cc171-d428-4369-81f2-551a7da10aee'),
       (uuid_generate_v4(), 'Рубль РФ', 35000, 'ed3cc171-d428-4369-81f2-551a7da10aee'),
       (uuid_generate_v4(), 'Доллар США', 1250, '1ab3308c-6bbc-4798-88e3-02176730d0ef'),
       (uuid_generate_v4(), 'Доллар США', 1700, 'c18cb5eb-0ad2-4c1d-9b4b-f8e2d8332b4c'),
       (uuid_generate_v4(), 'Евро', 750, 'c18cb5eb-0ad2-4c1d-9b4b-f8e2d8332b4c'),
       (uuid_generate_v4(), 'Доллар США', 1800, 'f8f7d151-7752-48e5-b47b-59505b41a36a'),
       (uuid_generate_v4(), 'Рубль РФ', 27000, 'f8f7d151-7752-48e5-b47b-59505b41a36a'),
       (uuid_generate_v4(), 'Доллар США', 1000, '0b08e81e-68de-48aa-b8a2-26dd3d24b9ba');

select c.name, c.surname, c.phone_number, a.currency_name, a.amount
from client c
join account a on c.id = a.client_id
where a.amount < 700
order by a.amount asc ;

select c.name, c.surname, c.phone_number, a.amount, a.currency_name
from client c
join account a ON c.id = a.client_id
where a.amount = (select min(amount) from account);

select sum(amount) as total_amount
from account;

select c.id, c.name, c.surname, c.phone_number, a.currency_name, a.amount
from client c
join account a on c.id = a.client_id;

select name, surname, date
from client
order by date asc;

select extract(year from age(date)) as age, count(*) as count
from client
group by age
having count(*) > 1
order by age asc;

select extract(year from age(date)) as age, count(*) as count, array_agg(c.name || ' ' || c.surname) AS clients
from client c
group by age
order by age asc;

select *
from client c
limit 10;
