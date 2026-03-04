-- --------------------------------------------------------
-- 호스트:                          127.0.0.1
-- 서버 버전:                        11.8.5-MariaDB - MariaDB Server
-- 서버 OS:                        Win64
-- HeidiSQL 버전:                  12.11.0.7065
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- berry_very 데이터베이스 구조 내보내기
CREATE DATABASE IF NOT EXISTS `berry_very` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */;
USE `berry_very`;

-- 테이블 berry_very.tb_device_ctrl 구조 내보내기
CREATE TABLE IF NOT EXISTS `tb_device_ctrl` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `gate_id` int(10) NOT NULL,
  `name` varchar(50) NOT NULL,
  `type` int(10) NOT NULL,
  `address` int(10) NOT NULL,
  `status` int(10) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_tb_device_ctrl_tb_device_gate` (`gate_id`),
  CONSTRAINT `FK_tb_device_ctrl_tb_device_gate` FOREIGN KEY (`gate_id`) REFERENCES `tb_device_gate` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 테이블 데이터 berry_very.tb_device_ctrl:~0 rows (대략적) 내보내기

-- 테이블 berry_very.tb_device_gate 구조 내보내기
CREATE TABLE IF NOT EXISTS `tb_device_gate` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `port_id` int(10) NOT NULL,
  `name` varchar(50) NOT NULL,
  `type` int(10) NOT NULL,
  `address` int(10) NOT NULL,
  `status` int(10) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_tb_device_gate_tb_device_port` (`port_id`),
  CONSTRAINT `FK_tb_device_gate_tb_device_port` FOREIGN KEY (`port_id`) REFERENCES `tb_device_port` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 테이블 데이터 berry_very.tb_device_gate:~0 rows (대략적) 내보내기

-- 테이블 berry_very.tb_device_port 구조 내보내기
CREATE TABLE IF NOT EXISTS `tb_device_port` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `type` int(10) NOT NULL,
  `address` varchar(50) NOT NULL,
  `status` int(10) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='통신 장비 연결 포트';

-- 테이블 데이터 berry_very.tb_device_port:~0 rows (대략적) 내보내기

-- 테이블 berry_very.tb_device_sub 구조 내보내기
CREATE TABLE IF NOT EXISTS `tb_device_sub` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `ctrl_id` int(10) NOT NULL,
  `name` varchar(50) NOT NULL DEFAULT '',
  `type` int(10) NOT NULL,
  `address` int(10) NOT NULL,
  `status` int(10) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_tb_device_sub_tb_device_ctrl` (`ctrl_id`),
  CONSTRAINT `FK_tb_device_sub_tb_device_ctrl` FOREIGN KEY (`ctrl_id`) REFERENCES `tb_device_ctrl` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 테이블 데이터 berry_very.tb_device_sub:~0 rows (대략적) 내보내기

-- 테이블 berry_very.tb_log 구조 내보내기
CREATE TABLE IF NOT EXISTS `tb_log` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `log` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 테이블 데이터 berry_very.tb_log:~0 rows (대략적) 내보내기

-- 테이블 berry_very.tb_status_ctrl 구조 내보내기
CREATE TABLE IF NOT EXISTS `tb_status_ctrl` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `ctrl_id` int(10) NOT NULL,
  `status` int(10) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_tb_status_ctrl_tb_device_ctrl` (`ctrl_id`),
  CONSTRAINT `FK_tb_status_ctrl_tb_device_ctrl` FOREIGN KEY (`ctrl_id`) REFERENCES `tb_device_ctrl` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 테이블 데이터 berry_very.tb_status_ctrl:~0 rows (대략적) 내보내기

-- 테이블 berry_very.tb_user_account 구조 내보내기
CREATE TABLE IF NOT EXISTS `tb_user_account` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `dept_id` int(10) unsigned NOT NULL,
  `rank_id` int(10) unsigned NOT NULL,
  `role_id` int(10) unsigned NOT NULL,
  `name` varchar(50) NOT NULL,
  `status` int(10) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_tb_user_account_tb_user_dept` (`dept_id`),
  KEY `FK_tb_user_account_tb_user_rank` (`rank_id`),
  KEY `FK_tb_user_account_tb_user_role` (`role_id`),
  CONSTRAINT `FK_tb_user_account_tb_user_dept` FOREIGN KEY (`dept_id`) REFERENCES `tb_user_dept` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_tb_user_account_tb_user_rank` FOREIGN KEY (`rank_id`) REFERENCES `tb_user_rank` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_tb_user_account_tb_user_role` FOREIGN KEY (`role_id`) REFERENCES `tb_user_role` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 테이블 데이터 berry_very.tb_user_account:~0 rows (대략적) 내보내기

-- 테이블 berry_very.tb_user_dept 구조 내보내기
CREATE TABLE IF NOT EXISTS `tb_user_dept` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `parent_id` int(10) unsigned DEFAULT NULL,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_tb_user_dept_tb_user_dept` (`parent_id`),
  CONSTRAINT `FK_tb_user_dept_tb_user_dept` FOREIGN KEY (`parent_id`) REFERENCES `tb_user_dept` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 테이블 데이터 berry_very.tb_user_dept:~0 rows (대략적) 내보내기

-- 테이블 berry_very.tb_user_rank 구조 내보내기
CREATE TABLE IF NOT EXISTS `tb_user_rank` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `parent_id` int(10) unsigned DEFAULT NULL,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_tb_user_rank_tb_user_rank` (`parent_id`),
  CONSTRAINT `FK_tb_user_rank_tb_user_rank` FOREIGN KEY (`parent_id`) REFERENCES `tb_user_rank` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 테이블 데이터 berry_very.tb_user_rank:~0 rows (대략적) 내보내기

-- 테이블 berry_very.tb_user_role 구조 내보내기
CREATE TABLE IF NOT EXISTS `tb_user_role` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `can_view` tinyint(3) unsigned NOT NULL,
  `can_create` tinyint(3) unsigned NOT NULL,
  `can_update` tinyint(3) unsigned NOT NULL,
  `can_delete` tinyint(3) unsigned NOT NULL,
  `can_control` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 테이블 데이터 berry_very.tb_user_role:~0 rows (대략적) 내보내기

-- 테이블 berry_very.tb_zone_bldg 구조 내보내기
CREATE TABLE IF NOT EXISTS `tb_zone_bldg` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 테이블 데이터 berry_very.tb_zone_bldg:~0 rows (대략적) 내보내기

-- 테이블 berry_very.tb_zone_floor 구조 내보내기
CREATE TABLE IF NOT EXISTS `tb_zone_floor` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `bldg_id` int(10) NOT NULL,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_tb_zone_floor_tb_zone_bldg` (`bldg_id`),
  CONSTRAINT `FK_tb_zone_floor_tb_zone_bldg` FOREIGN KEY (`bldg_id`) REFERENCES `tb_zone_bldg` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 테이블 데이터 berry_very.tb_zone_floor:~0 rows (대략적) 내보내기

-- 테이블 berry_very.tb_zone_room 구조 내보내기
CREATE TABLE IF NOT EXISTS `tb_zone_room` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `floor_id` int(10) NOT NULL,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_tb_zone_room_tb_zone_floor` (`floor_id`),
  CONSTRAINT `FK_tb_zone_room_tb_zone_floor` FOREIGN KEY (`floor_id`) REFERENCES `tb_zone_floor` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 테이블 데이터 berry_very.tb_zone_room:~0 rows (대략적) 내보내기

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
