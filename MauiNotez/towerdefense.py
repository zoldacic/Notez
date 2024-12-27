import pygame
import sys
import random

# Initialize Pygame
pygame.init()

# Screen dimensions
SCREEN_WIDTH = 800
SCREEN_HEIGHT = 600
screen = pygame.display.set_mode((SCREEN_WIDTH, SCREEN_HEIGHT))
pygame.display.set_caption("Tower Defense Game")

# Colors
WHITE = (255, 255, 255)
BLACK = (0, 0, 0)
GREEN = (0, 255, 0)
RED = (255, 0, 0)

# Clock
clock = pygame.time.Clock()

# Tower class
class Tower:
    def __init__(self, x, y):
        self.x = x
        self.y = y
        self.range = 100
        self.damage = 10
        self.cooldown = 60
        self.last_shot = 0

    def draw(self, screen):
        pygame.draw.circle(screen, GREEN, (self.x, self.y), 20)

    def shoot(self, enemies):
        if pygame.time.get_ticks() - self.last_shot > self.cooldown:
            for enemy in enemies:
                if self.in_range(enemy):
                    enemy.health -= self.damage
                    self.last_shot = pygame.time.get_ticks()
                    break

    def in_range(self, enemy):
        distance = ((self.x - enemy.x) ** 2 + (self.y - enemy.y) ** 2) ** 0.5
        return distance <= self.range

# Enemy class
class Enemy:
    def __init__(self, x, y):
        self.x = x
        self.y = y
        self.health = 100
        self.speed = 2

    def draw(self, screen):
        pygame.draw.rect(screen, RED, (self.x, self.y, 20, 20))

    def move(self):
        self.x += self.speed

# Main game loop
def main():
    towers = [Tower(400, 300)]
    enemies = [Enemy(0, random.randint(0, SCREEN_HEIGHT)) for _ in range(5)]

    while True:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                pygame.quit()
                sys.exit()

        screen.fill(WHITE)

        for tower in towers:
            tower.draw(screen)
            tower.shoot(enemies)

        for enemy in enemies:
            enemy.move()
            enemy.draw(screen)
            if enemy.health <= 0:
                enemies.remove(enemy)

        pygame.display.flip()
        clock.tick(60)

if __name__ == "__main__":
    main()
