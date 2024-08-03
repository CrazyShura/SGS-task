﻿# SGS task
 Тестовое техническое задание для разработчика Unity.

Тестовое состоит из двух блоков.
Первый блок - бой
Второй - перемещение по уровню


Блок первый:

Игра строится на механике получения и возвращения героем урона.
То есть – персонаж игрока получает урон от врага, какое-то время этот урон находится в «подвешенном состоянии», и, если герой не смог за это время нанести урон врагу, герой теряет часть жизни.

Для выполнения тестового задания требуется создать и прислать в согласованный срок сцену 2D в Unity (желательно версии 22.3.12f1), содержащую:

На платформе находятся два примитива (куб, капсула, спрайт, неважно) один из которых является героем (управляется игроком, а второй – врагом. У каждого из них есть палочка, отображающая оружие.
Персонаж игрока может передвигаться влево и вправо, совершать прыжок и производить атаку (после того как получил урон, в течение действия времени «подвешенного урона»).
В левом верхнем углу экрана есть 5 кружков – это здоровье героя.
В правом верхнем углу есть 3 кружка другого цвета, это здоровье врага.
Враг стоит на месте. Через равномерные промежутки времени враг атакует (взмахивает палкой) в направлении героя.
При получении героем урона один из его кружков-жизней начинает мигать. В этот момент герой может атаковать врага. До этого момента герою атака недоступна.
При успешной атаке по врагу, герою возвращается его «подвешенное» здоровье (перестаёт мигать), а у врага отнимается одно очко здоровья.
Герой может производить удар неоднократно, не попадая по врагу. При попадании по врагу способность атаки у героя отключается.
При пропуске времени «подвешенного» здоровья и не нанесении урона врагу, персонаж игрока теряет этот фрагмент здоровья.
При получении урона от врага в момент «подвешенного» здоровья персонаж игрока теряет сразу два очка здоровья.

При потере всех очков здоровья герой или враг исчезают.
Пожалуйста, расположите на экране сцены кнопку для перезапуска сцены (restart).


Блок второй:

На сцену добавить в произвольном виде "стену".
Герой должен быть способен, прыгнув на стену - медленно "сползать по ней под действием силы тяжести".
И должен уметь от неё отпрыгнуть.
Либо просто свалиться вниз.
Т.е. реализовать т.н. wall-jump


Так же отдельное пожелание:

Прошу вывести из кода настойки для регулирования:
Скорость атаки героя. 
Время «подвешенного урона» в секундах.
Скорость атаки врага.
Частота атаки врага.
Высота прыжка героя. 
Скорость перемещения героя влево-вправо.
Скорость "сползания по стене".

