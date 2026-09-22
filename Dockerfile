FROM ubuntu:24.04

# Установка необходимых системных библиотек для Headless Godot
RUN apt-get update && apt-get install -y \
    libfontconfig1 \
    libx11-6 \
    libxcursor1 \
    libxinerama1 \
    libgl1 \
    libglx-mesa0 \
    ca-certificates \
    && rm -rf /var/lib/apt/lists/*
WORKDIR /app

# Копируем скомпилированные файлы сервера
COPY build/ /app/

# Даем права на исполнение
RUN chmod +x /app/server.x86_64

# Запускаем Dedicated Server в headless-режиме
CMD ["./server.x86_64", "--headless"]